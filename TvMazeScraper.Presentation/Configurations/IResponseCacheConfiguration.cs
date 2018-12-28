namespace TvMazeScraper.Presentation.Configurations
{
    public interface IResponseCacheConfiguration
    {
        int MaxAgeInSeconds { get; set; }
    }
}