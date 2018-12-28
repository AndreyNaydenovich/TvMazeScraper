using System.Collections.Generic;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Presentation.Domain.Comparers
{
    public class CastComparer : Comparer<ICast>
    {
        public override int Compare(ICast x, ICast y)
        {
            if (x?.Birthday == null && y?.Birthday == null)
            {
                return 0;
            }
            if (x?.Birthday == null)
            {
                return 1;
            }
            if (y?.Birthday == null)
            {
                return -1;
            }

            return y.Birthday.Value.CompareTo(x.Birthday.Value);
        }
    }
}