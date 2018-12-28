using System.Threading.Tasks;
using StackExchange.Redis;
using TvMazeScraper.DAL.Configurations;

namespace TvMazeScraper.DAL
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public DatabaseFactory(DatabaseFactoryConfiguration config)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(config.ConnectionString);
        }

        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase();
        }
    }
}