using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TvMazeScraper.Presentation.Configurations;

namespace TvMazeScraper.Presentation.Middleware
{
    public class ApiResponseCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ResponseCacheConfiguration _config;

        public ApiResponseCachingMiddleware(RequestDelegate next, ResponseCacheConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value.StartsWith("/api/"))
            {
                SetResponseCache(httpContext);
            }

            return _next(httpContext);
        }

        private void SetResponseCache(HttpContext context)
        {
            context.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(_config.MaxAgeInSeconds)
            };
            context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { Microsoft.Net.Http.Headers.HeaderNames.AcceptEncoding };
        }
    }
}