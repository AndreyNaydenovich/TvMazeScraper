using System;
using Newtonsoft.Json;
using TvMazeScraper.Presentation.Helpers;

namespace TvMazeScraper.Presentation.Entities
{
    public class Cast
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime? Birthday { get; set; }
    }
}