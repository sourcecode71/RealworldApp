using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Web.Setting.GlobalExceptionHandling
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        int statusCode = context.Response.StatusCode;
                        string message = contextFeature.Error.Message + "." + contextFeature.Error.InnerException != null ? contextFeature.Error.InnerException.Message : "";

                        LoggingHelper.DatabaseLogger.Error($"Something went wrong.\n Status Code: {statusCode}.\nMessage: {message}");

                        //await context.Response.WriteAsync(new ErrorDetails()
                        //{
                        //    StatusCode = statusCode,
                        //    Message = message
                        //}.ToString());
                    }
                });
            });
        }
    }
}
