using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Contracts.Stores;
using TvMazeScraper.Presentation.Domain.Services;

namespace TvMazeScraper.Presentation.Domain.Tests.Services
{
    public class ShowServiceTest
    {
        private ShowService _sut;
        private Mock<IShowStore> _showStore;
        private Mock<IComparer<ICast>> _comparer;

        [SetUp]
        public void Setup()
        {
            _showStore = new Mock<IShowStore>();
            _comparer = new Mock<IComparer<ICast>>();
            _comparer.Setup(t => t.Compare(It.IsAny<ICast>(), It.IsAny<ICast>())).Returns(1);
            _sut = new ShowService(_showStore.Object, _comparer.Object);
        }

        [Test]
        public async Task Get_WithIdProvided_ShouldReturnNull()
        {
            _showStore.Setup(t => t.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<IShow>(null));

            var result = await _sut.GetAsync(1);

            result.Should().BeNull();
        }

        [Test]
        public async Task Get_WithIdProvided_ReturnsData()
        {
            var show = new Mock<IShow>();
            show.Setup(t => t.Cast).Returns(new List<ICast>());
            _showStore.Setup(t => t.GetAsync(It.Is<int>(v => v == 1))).Returns(Task.FromResult(show.Object));

            var result = await _sut.GetAsync(1);

            result.Should().NotBeNull();
        }

        [Test]
        public async Task Get_WithOffsetLimitProvided_ReturnsData()
        {
            var showCount = 2;
            var shows = new List<IShow>(showCount);
            for (var i = 0; i < showCount; i++)
            {
                var show = new Mock<IShow>();
                show.Setup(t => t.Cast).Returns(new List<ICast>());
                shows.Add(show.Object);
            }

            _showStore.Setup(t => t.GetAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((offset, limit) => Task.FromResult(shows.Skip(offset).Take(limit).ToList()));

            var result = await _sut.GetAsync(0, shows.Count);
            result.Should().NotBeNull();
            result.Count.Should().Be(shows.Count);

            result = await _sut.GetAsync(0, shows.Count - 1);
            result.Should().NotBeNull();
            result.Count.Should().Be(shows.Count - 1);

            result = await _sut.GetAsync(shows.Count, 5);
            result.Should().NotBeNull();
            result.Count.Should().Be(0);

            result = await _sut.GetAsync(1, shows.Count);
            result.Should().NotBeNull();
            result.Count.Should().Be(shows.Count - 1);
        }
    }
}