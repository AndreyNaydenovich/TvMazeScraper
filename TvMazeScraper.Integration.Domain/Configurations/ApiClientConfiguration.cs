﻿namespace TvMazeScraper.Integration.Domain.Configurations
{
    public class ApiClientConfiguration
    {
        public int DelayInMilliseconds { get; set; }
        public int MaxRetryCount { get; set; }
        public string UpdateEndpoint { get; set; }
        public string ShowEndpoint { get; set; }
    }
}