using System.Collections.Generic;

namespace TvMazeScraper.Presentation.Entities
{
    public class Show
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Cast> Cast { get; set; }
    }
}