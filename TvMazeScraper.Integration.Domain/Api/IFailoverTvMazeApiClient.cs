using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Integration.Domain.Api
{
    public interface IFailoverTvMazeApiClient
    {
        Task<Dictionary<int, int>> GetUpdateListAsync(CancellationToken cancellationToken);
        Task<IShow> GetShowInfoAsync(int id, CancellationToken cancellationToken);
    }
}