using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TvMazeScraper.Integration.Domain.Api;
using TvMazeScraper.Integration.Domain.Configurations;

namespace TvMazeScraper.Integration.Domain.Tests.Api
{
    public class FailoverTvMazeApiClientTest
    {
        private FailoverTvMazeApiClient _sut;
        private Mock<ITvMazeApiClient> _apiClient;

        [SetUp]
        public void Setup()
        {
            _apiClient = new Mock<ITvMazeApiClient>();
            var config = new ApiClientConfiguration
            {
                DelayInMilliseconds = 200,
                MaxRetryCount = 20
            };
            _sut = new FailoverTvMazeApiClient(_apiClient.Object, config);
        }

        [Test]
        public async Task UpdateListRetrieval_WithCancelationTokenProvided_ShouldReturnNull()
        {
            var result = await _sut.GetUpdateListAsync(new CancellationToken(true));

            result.Should().BeNull();
        }

        [Test]
        public async Task UpdateListRetrieval_WithNoLimitExceeds_ReturnsData()
        {
            var testData = new Dictionary<int, int> { { 1, 1 } };
            (bool IsRateLimitExceed, Dictionary<int, int> data) testObj = (false, testData);
            _apiClient.Setup(x => x.GetUpdateListAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(testObj));

            var result = await _sut.GetUpdateListAsync(new CancellationToken(false));

            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            result.Should().Equal(testData);
        }
    }
}