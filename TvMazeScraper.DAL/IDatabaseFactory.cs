using StackExchange.Redis;

namespace TvMazeScraper.DAL
{
    public interface IDatabaseFactory
    {
        IDatabase GetDatabase();
    }
}