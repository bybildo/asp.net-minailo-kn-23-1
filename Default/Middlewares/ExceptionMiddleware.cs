using Default.Application.Exceptions;
using Default.Application.Exeptions;
using DicrtionaryAPI.Presentation.Middlewares;
using System.Net;
using System.Text.Json;

namespace DicrtionaryAPI.Presentation.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                IncorrectDataEnteredException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var message = statusCode == HttpStatusCode.InternalServerError ? "An internal server error occurred." : exception.Message;

            var result = JsonSerializer.Serialize(new
            {
                status = context.Response.StatusCode,
                message = message
            });

            return context.Response.WriteAsync(result);
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}