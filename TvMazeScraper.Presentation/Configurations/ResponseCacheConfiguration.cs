namespace TvMazeScraper.Presentation.Configurations
{
    public class ResponseCacheConfiguration : IResponseCacheConfiguration
    {
        public int MaxAgeInSeconds { get; set; }
    }
}