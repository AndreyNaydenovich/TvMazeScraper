using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Presentation.Entities;

namespace TvMazeScraper.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DAL.Entities.Show, Show>();
                cfg.CreateMap<DAL.Entities.Cast, Cast>();
            });

            services.AddSingleton<IMapper>(sp => config.CreateMapper());
        }
    }
}