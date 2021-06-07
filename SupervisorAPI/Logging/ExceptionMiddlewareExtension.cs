using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SupervisorAPI.Model;
using System.Net;

namespace SupervisorAPI.Logging
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILog logger)
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
                        logger.Error($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ConfirmationResponse()
                        {
                            OrderID = "",
                            AgentId = "",
                            OrderStatus = "",
                            StatusCode = context.Response.StatusCode,
                            FaultMessage = "Something went wrong .Internal Server Error!"
                        }.ToString());
                    }
                });
            });
        }
    }
}
