using Newtonsoft.Json;
using System;

namespace TvMazeScraper.Presentation.Entities
{
    public class Cast
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthday")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime? Birthday { get; set; }
    }
}