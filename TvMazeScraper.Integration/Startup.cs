using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TvMazeScraper.Integration.Domain;
using TvMazeScraper.Integration.Domain.Configurations;
using TvMazeScraper.Integration.Extensions;
using TvMazeScraper.Integration.Jobs;

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
            services.AddSingleton<IFailoverTvMazeApiClientConfiguration>(Configuration.GetSection("ApiClientConfiguration").Get<ApiClientConfiguration>());
            services.AddSingleton<DAL.IDatabaseFactoryConfiguration>(Configuration.GetSection("DatabaseFactoryConfiguration").Get<DatabaseFactoryConfiguration>());

            services.AddLogging();
            services.AddApplicationInsightsTelemetry();

            services.AddAutoMapper();

            services.AddSingleton<DAL.IDatabaseFactory, DAL.DatabaseFactory>();
            services.AddSingleton<DAL.ISerializer, DAL.Helpers.Serializer>();
            services.AddTransient<Contracts.IKeyValueStore, DAL.KeyValueStore>();
            services.AddTransient<Contracts.IShowStore, DAL.ShowStore>();
            services.AddTransient<ITvMazeApiClient, TvMazeApiClient>();
            services.AddTransient<IFailoverTvMazeApiClient, FailoverTvMazeApiClient>();
            services.AddTransient<ShowSynchronization>();

            services.AddHostedService<ShowSynchronizationHostedService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddApplicationInsights(serviceProvider);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}