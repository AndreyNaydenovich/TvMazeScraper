namespace TvMazeScraper.Integration.Domain.Configurations
{
    public interface IApiClientConfiguration
    {
        int DelayInMilliseconds { get; set; }
        int MaxRetryCount { get; set; }
        string UpdateEndpoint { get; set; }
        string ShowEndpoint { get; set; }
    }
}