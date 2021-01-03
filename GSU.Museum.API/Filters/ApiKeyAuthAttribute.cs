using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;

namespace GSU.Museum.API.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string _apiKeyName = "X-API-KEY";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(_apiKeyName, out var receivedApiKey))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.HttpContext.Response.ContentType = "application/json";
                var error = new Error(Errors.Unauthorized, "No api key in header");
                Log.Error($"Exception: {error}");
                await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(error));
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>(_apiKeyName);

            if (!apiKey.Equals(receivedApiKey))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.HttpContext.Response.ContentType = "application/json";
                var error = new Error(Errors.Unauthorized, "Incorrect api key");
                Log.Error($"Exception: {error}");
                await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(error));
                return;
            }

            await next();
        }
    }
}
