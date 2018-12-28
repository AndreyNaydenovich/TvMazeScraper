using System.Collections.Generic;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class SynchronizationServiceState
    {
        public int Timestamp { get; set; }

        public List<int> ShowIds { get; set; }

        public SynchronizationServiceState()
        {
            ShowIds = new List<int>();
        }
    }
}