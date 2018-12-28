using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Presentation.Domain.Services
{
    public interface IShowService
    {
        Task<IShow> GetAsync(int id);
        Task<IEnumerable<IShow>> GetAsync(int offset, int count);
    }
}