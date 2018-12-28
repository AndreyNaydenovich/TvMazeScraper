using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Integration.Domain.Configurations;
using TvMazeScraper.Integration.Domain.Exceptions;

namespace TvMazeScraper.Integration.Domain.Api
{
    public class FailoverTvMazeApiClient : IFailoverTvMazeApiClient
    {
        private readonly ITvMazeApiClient _apiClient;
        private readonly IApiClientConfiguration _config;

        public FailoverTvMazeApiClient(ITvMazeApiClient apiClient, IApiClientConfiguration config)
        {
            _apiClient = apiClient;
            _config = config;
        }

        public async Task<Dictionary<int, int>> GetUpdateListAsync(CancellationToken cancellationToken)
        {
            var retryCount = 0;

            while (true)
            {
                var result = await _apiClient.GetUpdateListAsync(cancellationToken).ConfigureAwait(false);

                if (result.IsRateLimitExceed)
                {
                    retryCount = await WaitForRetryAsync(retryCount, cancellationToken);
                }
                else
                {
                    return result.Data;
                }
            }
        }

        public async Task<IShow> GetShowInfoAsync(int id, CancellationToken cancellationToken)
        {
            var retryCount = 0;

            while (true)
            {
                var result = await _apiClient.GetShowInfoAsync(id, cancellationToken);

                if (result.IsRateLimitExceed)
                {
                    retryCount = await WaitForRetryAsync(retryCount, cancellationToken);
                }
                else
                {
                    return result.Data;
                }
            }
        }

        public async Task<int> WaitForRetryAsync(int retryCount, CancellationToken cancellationToken)
        {
            if (retryCount > _config.MaxRetryCount)
            {
                throw new RateLimitExceedException();
            }

            await Task.Delay(++retryCount * _config.DelayInMilliseconds, cancellationToken);
            return retryCount;
        }
    }
}