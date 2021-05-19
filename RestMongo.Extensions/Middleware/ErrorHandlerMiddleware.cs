using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using RestMongo.Domain.Abstractions.Exceptions;

namespace RestMongo.Extensions.Middleware
{
    public static class CustomErrorHandlerHelper
    {
        public static void UseCustomErrors(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.Use(WriteDevelopmentResponse);
            }
            else
            {
                app.Use(WriteProductionResponse);
            }
        }

        private static Task WriteDevelopmentResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, includeDetails: true);

        private static Task WriteProductionResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, includeDetails: false);

        private static async Task WriteResponse(HttpContext httpContext, bool includeDetails)
        {
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;
            if (ex != null)
            {
                httpContext.Response.ContentType = "application/json";

                var title = "An error occured: " + ex.Message + "An error occured";
                var details = includeDetails ? ex.ToString() : null;
                var statusCode = 500;
                HttpStatusCodeException statusCodeException = ex as HttpStatusCodeException;
                if (statusCodeException != null)
                {
                    statusCode = statusCodeException.HttpStatusCode;
                }
                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = details,
                    Type = ex.GetType().FullName,
                };
                var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
                if (traceId != null)
                {
                    problem.Extensions["traceId"] = traceId;
                }
                httpContext.Response.StatusCode = problem.Status.Value;
                var stream = httpContext.Response.Body;
                await JsonSerializer.SerializeAsync(stream, problem);
            }
        }
    }
}