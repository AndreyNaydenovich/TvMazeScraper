using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Integration.Domain.Api
{
    public interface ITvMazeApiClient
    {
        Task<(bool IsRateLimitExceed, IShow Data)> GetShowInfoAsync(int id, CancellationToken cancellationToken);
        Task<(bool IsRateLimitExceed, Dictionary<int, int> Data)> GetUpdateListAsync(CancellationToken cancellationToken);
    }
}