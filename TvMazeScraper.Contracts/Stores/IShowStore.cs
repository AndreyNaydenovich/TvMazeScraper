using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Contracts.Stores
{
    public interface IShowStore
    {
        Task SetAsync(IShow show);
        Task<IShow> GetAsync(int id);
        Task<IEnumerable<IShow>> GetAsync(int offset, int count);
        Task RemoveNotMatchedAsync(IEnumerable<int> ids);
    }
}