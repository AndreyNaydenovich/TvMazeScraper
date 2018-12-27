using System.Collections.Generic;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Presentation.Domain
{
    public class CastComparer : Comparer<ICast>, IComparer<ICast>
    {
        public override int Compare(ICast x, ICast y)
        {
            if (x?.Birthday == null && y?.Birthday == null)
            {
                return 0;
            }
            else if (x?.Birthday == null)
            {
                return 1;
            }
            else if (y?.Birthday == null)
            {
                return -1;
            }
            else
            {
                return y.Birthday.Value.CompareTo(x.Birthday.Value);
            }
        }
    }
}