using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Contracts;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Presentation.Domain
{
    public class SortedShowStore : ISortedShowStore
    {
        private readonly IShowStore _showStore;
        private readonly IComparer<ICast> _castComparer;

        public SortedShowStore(IShowStore showStore, IComparer<ICast> castComparer)
        {
            _showStore = showStore;
            _castComparer = castComparer;
        }

        public async Task<IShow> GetAsync(int id)
        {
            var show = await _showStore.GetAsync(id);
            show.Cast.Sort(_castComparer);
            return show;
        }

        public async Task<List<IShow>> GetAsync(int offset, int count)
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