using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TvMazeScraper.Integration.Domain.Helpers
{
    public class FailsafeDateConverter : DateTimeConverterBase
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dateString = reader.Value as string;
            if (string.IsNullOrEmpty(dateString))
            {
                return default(DateTime?);
            }

            if (!DateTime.TryParse(dateString, out var date))
            {
                return default(DateTime?);
            }

            return date;
        }
    }
}