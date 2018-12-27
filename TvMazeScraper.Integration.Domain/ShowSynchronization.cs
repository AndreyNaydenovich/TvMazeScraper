using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Contracts;
using TvMazeScraper.Integration.Domain.Entities;

namespace TvMazeScraper.Integration.Domain
{
    public class ShowSynchronization : IDisposable
    {
        private readonly IKeyValueStore _keyValueStore;
        private readonly IShowStore _showStore;
        private readonly IFailoverTvMazeApiClient _apiClient;
        private readonly ILogger<ShowSynchronization> _logger;

        public ShowSynchronization(IKeyValueStore keyValueStore, IShowStore showStore, IFailoverTvMazeApiClient apiClient, ILogger<ShowSynchronization> logger)
        {
            _keyValueStore = keyValueStore;
            _showStore = showStore;
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task StartSynchronizationAsync(CancellationToken cancellationToken)
        {
            var updateList = await _apiClient.GetUpdateListAsync(cancellationToken).ConfigureAwait(false);
            await _showStore.RemoveNotMatchedAsync(updateList.Keys).ConfigureAwait(false);
            await UpdateShowsAsync(updateList, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateShowsAsync(Dictionary<int, int> updateList, CancellationToken cancellationToken)
        {
            var statusKey = "JobStatus";

            var lastStatus = await _keyValueStore.GetAsync<ShowSynchronizationStatus>(statusKey) ?? new ShowSynchronizationStatus();

            var currentStatus = new ShowSynchronizationStatus();

            foreach (var showToUpdate in updateList.OrderBy(t => t.Value))
            {
                if (cancellationToken.CanBeCanceled && cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (showToUpdate.Value < lastStatus.Date ||
                    showToUpdate.Value == lastStatus.Date && lastStatus.ShowIds.Contains(showToUpdate.Key))
                {
                    continue;
                }

                var show = await _apiClient.GetShowInfoAsync(showToUpdate.Key, cancellationToken);
                await _showStore.SetAsync(show);

                if (currentStatus.Date == showToUpdate.Value)
                {
                    currentStatus.ShowIds.Add(showToUpdate.Key);
                }
                else
                {
                    currentStatus.Date = showToUpdate.Value;
                    currentStatus.ShowIds.Clear();
                    currentStatus.ShowIds.Add(showToUpdate.Key);
                }

                await _keyValueStore.SetAsync(statusKey, currentStatus);
                _logger.LogInformation($@"Show #{show.Id} is updated.");
            }
        }

        public void Dispose()
        {
            _apiClient.Dispose();
        }
    }
}