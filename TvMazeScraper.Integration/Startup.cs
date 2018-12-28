using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TvMazeScraper.Contracts.Stores;
using TvMazeScraper.Integration.Configurations;
using TvMazeScraper.Integration.Domain;
using TvMazeScraper.Integration.Domain.Api;
using TvMazeScraper.Integration.Domain.Configurations;
using TvMazeScraper.Integration.Extensions;
using TvMazeScraper.Integration.Services;

namespace TvMazeScraper.Integration
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISynchronizationConfiguration>(Configuration.GetSection("SynchronizationConfiguration").Get<SynchronizationConfiguration>());
            services.AddSingleton<IApiClientConfiguration>(Configuration.GetSection("ApiClientConfiguration").Get<ApiClientConfiguration>());
            services.AddSingleton<DAL.Configurations.IDatabaseFactoryConfiguration>(Configuration.GetSection("DatabaseFactoryConfiguration")
                .Get<DAL.Configurations.DatabaseFactoryConfiguration>());

            services.AddLogging();
            services.AddApplicationInsightsTelemetry();

            services.AddAutoMapper();
            services.AddHttpClient<ITvMazeApiClient, TvMazeApiClient>();
            services.AddSingleton<DAL.IDatabaseFactory, DAL.DatabaseFactory>();
            services.AddSingleton<DAL.Helpers.ISerializer, DAL.Helpers.Serializer>();
            services.AddTransient<IKeyValueStore, DAL.Stores.KeyValueStore>();
            services.AddTransient<IShowStore, DAL.Stores.ShowStore>();
            services.AddTransient<IFailoverTvMazeApiClient, FailoverTvMazeApiClient>();
            services.AddTransient<ISynchronizationService, SynchronizationService>();

            services.AddHostedService<SynchronizationHostedService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddApplicationInsights(serviceProvider);
        }
    }
}