using GSU.Museum.API.Data.Enums;
using GSU.Museum.API.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace GSU.Museum.API.Extensions
{
    /// <summary>
    /// Global exception handler. Catches exceptions and send user error model
    /// </summary>
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.HttpContext.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error.GetType() == typeof(Error))
                        {
                            Error error = contextFeature.Error as Error;
                            Log.Error($"Exception: {contextFeature.Error}");
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
                        }
                        else
                        {
                            Log.Fatal($"Exception: {contextFeature.Error}");
                            await context.Response.WriteAsync(new Error() { ErrorCode = Errors.Unhandled_exception, Info = contextFeature.Error.Message }.ToString());
                        }
                    }
                });
            });
        }
    }
}
