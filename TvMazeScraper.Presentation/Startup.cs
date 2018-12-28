using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Contracts.Stores;
using TvMazeScraper.Presentation.Configurations;
using TvMazeScraper.Presentation.Domain.Comparers;
using TvMazeScraper.Presentation.Extensions;
using TvMazeScraper.Presentation.Middleware;

namespace TvMazeScraper.Presentation
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
            services.AddSingleton(Configuration.GetSection("ResponseCache").Get<ResponseCacheConfiguration>());
            services.AddSingleton(Configuration.GetSection("DatabaseFactoryConfiguration").Get<DAL.Configurations.DatabaseFactoryConfiguration>());

            services.AddLogging();
            services.AddApplicationInsightsTelemetry();

            services.AddAutoMapper();

            services.AddSingleton<DAL.IDatabaseFactory, DAL.DatabaseFactory>();
            services.AddSingleton<DAL.Helpers.ISerializer, DAL.Helpers.Serializer>();
            services.AddTransient<IKeyValueStore, DAL.Stores.KeyValueStore>();
            services.AddTransient<IShowStore, DAL.Stores.ShowStore>();
            services.AddSingleton<IComparer<Contracts.Entities.ICast>, CastComparer>();
            services.AddTransient<Domain.Services.IShowService, Domain.Services.ShowService>();

            services.AddResponseCaching();
            services.AddResponseCompression();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerDocument();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ErrorHandlingMiddleware>();
            }

            app.UseResponseCaching();
            app.UseMiddleware<ApiResponseCachingMiddleware>();
            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseMvc();
        }
    }
}
