using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MomitKiller.Api.Middleware
{
    public class ApiKeyAuthentication
    {
        private readonly RequestDelegate _next;
        private string _apiKey;

        public ApiKeyAuthentication(RequestDelegate next, Settings settings)
        {
            _next = next;
            _apiKey = settings.GetValue<string>("ApiKey");
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains("api-key"))
            {
                context.Response.StatusCode = 400;                
                await context.Response.WriteAsync("Api Key is missing");
                return;
            }
            else
            {
                if (_apiKey != context.Request.Headers["api-key"])
                {
                    context.Response.StatusCode = 401; 
                    await context.Response.WriteAsync("Invalid Api Key");
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }

    #region ExtensionMethod
    public static class ApiKeyAuthenticationExtension
    {
        public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyAuthentication>();
            return app;
        }
    }
    #endregion
}
