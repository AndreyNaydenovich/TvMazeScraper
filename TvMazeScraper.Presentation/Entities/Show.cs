using Newtonsoft.Json;
using System.Collections.Generic;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Presentation.Entities
{
    public class Show
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cast")]
        public List<Cast> Cast { get; set; }
    }
}