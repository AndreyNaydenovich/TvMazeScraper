namespace TvMazeScraper.Integration.Domain.Configurations
{
    public class ApiClientConfiguration : IFailoverTvMazeApiClientConfiguration
    {
        public int DelayInMilliseconds { get; set; }
    }
}