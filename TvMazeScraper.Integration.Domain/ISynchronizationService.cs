using System.Threading;
using System.Threading.Tasks;

namespace TvMazeScraper.Integration.Domain
{
    public interface ISynchronizationService
    {
        Task StartSynchronizationAsync(CancellationToken cancellationToken);
    }
}