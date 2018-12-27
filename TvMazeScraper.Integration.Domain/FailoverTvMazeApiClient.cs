using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Integration.Domain
{
    public interface IFailoverTvMazeApiClient : IDisposable
    {
        Task<Dictionary<int, int>> GetUpdateListAsync(CancellationToken cancellationToken);
        Task<IShow> GetShowInfoAsync(int id, CancellationToken cancellationToken);
    }

    public class FailoverTvMazeApiClient : IFailoverTvMazeApiClient
    {
        private readonly ITvMazeApiClient _apiClient;
        private readonly int _requestDelay;

        public FailoverTvMazeApiClient(ITvMazeApiClient apiClient, IFailoverTvMazeApiClientConfiguration config)
        {
            _apiClient = apiClient;
            _requestDelay = config.DelayInMilliseconds;
        }

        public async Task<Dictionary<int, int>> GetUpdateListAsync(CancellationToken cancellationToken)
        {
            var errorQty = 0;

            while (true)
            {
                if (cancellationToken.CanBeCanceled && cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                var result = await _apiClient.GetUpdateListAsync(cancellationToken).ConfigureAwait(false);

                if (result.IsRateLimitExceed)
                {
                    await Task.Delay(++errorQty * _requestDelay, cancellationToken);
                }
                else
                {
                    return result.data;
                }
            }
        }

        public async Task<IShow> GetShowInfoAsync(int id, CancellationToken cancellationToken)
        {
            var errorQty = 0;

            while (true)
            {
                var result = await _apiClient.GetShowInfoAsync(id, cancellationToken);

                if (result.IsRateLimitExceed)
                {
                    await Task.Delay(++errorQty * _requestDelay, cancellationToken);
                }
                else
                {
                    return result.data;
                }
            }
        }

        public void Dispose()
        {
            _apiClient.Dispose();
        }
    }
}