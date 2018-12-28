using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Contracts.Stores;

namespace TvMazeScraper.Presentation.Domain.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowStore _showStore;
        private readonly IComparer<ICast> _castComparer;

        public ShowService(IShowStore showStore, IComparer<ICast> castComparer)
        {
            _showStore = showStore;
            _castComparer = castComparer;
        }

        public async Task<IShow> GetAsync(int id)
        {
            var show = await _showStore.GetAsync(id);

            if (show == null)
            {
                return null;
            }

            show.Cast.Sort(_castComparer);
            return show;
        }

        public async Task<IEnumerable<IShow>> GetAsync(int offset, int count)
        {
            var showList = await _showStore.GetAsync(offset, count);

            foreach (var show in showList)
            {
                show.Cast.Sort(_castComparer);
            }

            return showList;
        }
    }
}