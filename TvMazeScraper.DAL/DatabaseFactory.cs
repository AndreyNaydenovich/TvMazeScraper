using StackExchange.Redis;

namespace TvMazeScraper.DAL
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private ConnectionMultiplexer _connectionMultiplexer;

        public DatabaseFactory(IDatabaseFactoryConfiguration config)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(config.ConnectionString);
        }

        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase();
        }
    }
}