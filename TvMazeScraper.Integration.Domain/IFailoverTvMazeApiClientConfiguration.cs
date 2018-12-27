namespace TvMazeScraper.Integration.Domain
{
    public interface IFailoverTvMazeApiClientConfiguration
    {
        int DelayInMilliseconds { get; set; }
    }
}