using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Integration.Domain
{
    public interface ITvMazeApiClient : IDisposable
    {
        Task<(bool IsRateLimitExceed, IShow data)> GetShowInfoAsync(int id, CancellationToken cancellationToken);
        Task<(bool IsRateLimitExceed, Dictionary<int, int> data)> GetUpdateListAsync(CancellationToken cancellationToken);
    }
}