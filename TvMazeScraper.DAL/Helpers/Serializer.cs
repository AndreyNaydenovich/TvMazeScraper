using Newtonsoft.Json;

namespace TvMazeScraper.DAL.Helpers
{
    public class Serializer : ISerializer
    {
        private JsonSerializerSettings _settings;

        public Serializer()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T Deserialize<T>(string serializedObj)
        {
            return JsonConvert.DeserializeObject<T>(serializedObj, _settings);
        }
    }
}