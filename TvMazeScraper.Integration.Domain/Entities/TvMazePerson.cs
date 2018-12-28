using System;
using Newtonsoft.Json;
using TvMazeScraper.Integration.Domain.Helpers;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class TvMazePerson
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthday")]
        [JsonConverter(typeof(FailsafeDateConverter))]
        public DateTime? Birthday { get; set; }
    }
}