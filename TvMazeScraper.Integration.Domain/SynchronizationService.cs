using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TvMazeScraper.Contracts.Stores;
using TvMazeScraper.Integration.Domain.Api;
using TvMazeScraper.Integration.Domain.Entities;

namespace TvMazeScraper.Integration.Domain
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly IKeyValueStore _keyValueStore;
        private readonly IShowStore _showStore;
        private readonly IFailoverTvMazeApiClient _apiClient;
        private readonly ILogger<SynchronizationService> _logger;

        public SynchronizationService(IKeyValueStore keyValueStore, IShowStore showStore, IFailoverTvMazeApiClient apiClient, ILogger<SynchronizationService> logger)
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
            var stateKey = "SynchronizationServiceState";

            var lastState = await _keyValueStore.GetAsync<SynchronizationServiceState>(stateKey) ?? new SynchronizationServiceState();

            var currentState = new SynchronizationServiceState();

            foreach (var showToUpdate in updateList.OrderBy(t => t.Value))
            {
                if (showToUpdate.Value < lastState.Timestamp ||
                    showToUpdate.Value == lastState.Timestamp && lastState.ShowIds.Contains(showToUpdate.Key))
                {
                    continue;
                }

                var show = await _apiClient.GetShowInfoAsync(showToUpdate.Key, cancellationToken);
                await _showStore.SetAsync(show);

                if (currentState.Timestamp == showToUpdate.Value)
                {
                    currentState.ShowIds.Add(showToUpdate.Key);
                }
                else
                {
                    currentState.Timestamp = showToUpdate.Value;
                    currentState.ShowIds.Clear();
                    currentState.ShowIds.Add(showToUpdate.Key);
                }

                await _keyValueStore.SetAsync(stateKey, currentState);
                _logger.LogInformation($@"Show #{show.Id} is updated.");
            }
        }
    }
}