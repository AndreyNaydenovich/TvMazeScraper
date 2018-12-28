using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TvMazeScraper.Integration.Configurations;
using TvMazeScraper.Integration.Domain;

namespace TvMazeScraper.Integration.Services
{
    internal class SynchronizationHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly ISynchronizationService _synchronizationService;
        private readonly SynchronizationConfiguration _config;

        private Timer _timer;
        private bool _workInProgress = false;
        private readonly object _workInProgressLock = new object();

        public SynchronizationHostedService(ILogger<SynchronizationHostedService> logger, ISynchronizationService synchronizationService, SynchronizationConfiguration config)
        {
            _logger = logger;
            _synchronizationService = synchronizationService;
            _config = config;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Show Synchronization Service is starting.");

            _timer = new Timer(async state => { await DoWorkAsync((CancellationToken)state); }, cancellationToken, TimeSpan.Zero, TimeSpan.FromSeconds(_config.PeriodInSeconds));

            return Task.CompletedTask;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            lock (_workInProgressLock)
            {
                if (_workInProgress)
                {
                    return;
                }
                _workInProgress = true;
            }

            _logger.LogInformation("Show synchronization started.");

            try
            {
                await _synchronizationService.StartSynchronizationAsync(cancellationToken);

                _logger.LogInformation("Show synchronization finished.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Show synchronization finished with error.");
            }
            finally
            {
                lock (_workInProgressLock)
                {
                    _workInProgress = false;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Show Synchronization Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}