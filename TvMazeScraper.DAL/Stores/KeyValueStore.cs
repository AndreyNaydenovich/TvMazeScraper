using StackExchange.Redis;
using System.Threading.Tasks;
using TvMazeScraper.Contracts;

namespace TvMazeScraper.DAL
{
    public class KeyValueStore : IKeyValueStore
    {
        private readonly IDatabase _db;
        private readonly ISerializer _serializer;
        private const string RegionName = "KeyValueStore:";

        public KeyValueStore(IDatabaseFactory databaseFactory, ISerializer serializer)
        {
            _db = databaseFactory.GetDatabase();
            _serializer = serializer;
        }

        public Task SetAsync<T>(string key, T value)
        {
            var serializedValue = _serializer.Serialize(value);
            return _db.StringSetAsync(RegionName + key, serializedValue, flags: CommandFlags.DemandMaster);
        }

        public Task RemoveAsync(string key)
        {
            return _db.KeyDeleteAsync(RegionName + key, CommandFlags.DemandMaster);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var redisValue = await _db.StringGetAsync(RegionName + key).ConfigureAwait(false);

            if (redisValue.HasValue)
                return _serializer.Deserialize<T>(redisValue);

            return default(T);
        }
    }
}