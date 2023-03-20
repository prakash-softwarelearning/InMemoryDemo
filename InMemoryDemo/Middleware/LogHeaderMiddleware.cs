using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InMemoryDemo.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LogHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogHeaderMiddleware> _logger;

        public LogHeaderMiddleware(ILogger<LogHeaderMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            this._logger = logger;
        }

        //public Task Invoke(HttpContext httpContext)
        //{

        //    return _next(httpContext);
        //}
        public async Task InvokeAsync(HttpContext context)
        {
            var header = context.Request.Headers["CorrelationId"];
            //_logger.LogInformation("CorrelationId {header}", header);
            if (header.Count > 0)
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<LogHeaderMiddleware>>();
                using (logger.BeginScope("{@CorrelationId}", header[0]))
                {
                    _logger.LogInformation("{@CorrelationId}", header[0]);
                    await this._next(context);
                }
            }
            else
            {
                await this._next(context);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LogHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogHeaderMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogHeaderMiddleware>();
        }
    }
}
