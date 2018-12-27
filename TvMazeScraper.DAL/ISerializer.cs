namespace TvMazeScraper.DAL
{
    public interface ISerializer
    {
        T Deserialize<T>(string serializedObj);
        string Serialize<T>(T obj);
    }
}