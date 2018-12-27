using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Presentation.Domain
{
    public interface ISortedShowStore
    {
        Task<IShow> GetAsync(int id);
        Task<List<IShow>> GetAsync(int offset, int count);
    }
}