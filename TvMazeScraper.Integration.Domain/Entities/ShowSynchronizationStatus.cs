using System.Collections.Generic;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class ShowSynchronizationStatus
    {
        public int Date { get; set; }
        public int Timestamp { get => Date; set => Date = value; }

        public List<int> ShowIds { get; set; }

        public ShowSynchronizationStatus()
        {
            ShowIds = new List<int>();
        }
    }
}