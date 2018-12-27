using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using TvMazeScraper.Integration.Domain.Configurations;
using TvMazeScraper.Presentation.Configurations;
using TvMazeScraper.Presentation.Extensions;

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
            services.AddSingleton<ResponseCacheConfiguration>(Configuration.GetSection("ResponseCache").Get<ResponseCacheConfiguration>());
            services.AddSingleton<DAL.IDatabaseFactoryConfiguration>(Configuration.GetSection("DatabaseFactoryConfiguration").Get<DatabaseFactoryConfiguration>());

            services.AddLogging();
            services.AddApplicationInsightsTelemetry();

            services.AddAutoMapper();

            services.AddSingleton<DAL.IDatabaseFactory, DAL.DatabaseFactory>();
            services.AddSingleton<DAL.ISerializer, DAL.Helpers.Serializer>();
            services.AddTransient<Contracts.IKeyValueStore, DAL.KeyValueStore>();
            services.AddTransient<Contracts.IShowStore, DAL.ShowStore>();
            services.AddTransient<Domain.ISortedShowStore, Domain.SortedShowStore>();
            services.AddSingleton<IComparer<Contracts.Entities.ICast>, Domain.CastComparer>();

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

            app.UseResponseCaching();
            //app.UseResponseCacheConfigurationMiddleware();
            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUi3();
            //app.UseSwaggerUi3(settings =>
            //{
            //    settings.GeneratorSettings.DefaultPropertyNameHandling =
            //        PropertyNameHandling.CamelCase;
            //});

            app.UseMvc();
        }
    }
}
