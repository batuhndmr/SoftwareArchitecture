using System.Net;
using System.Text.Json;
using SoftwareArchitecture.Api.Models;
using SoftwareArchitecture.Application.Exceptions;

namespace SoftwareArchitecture.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                await WriteErrorAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); 
                
                _logger.LogError(ex, "Unhandled exception");
                await WriteErrorAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Unexpected error occurred"
                );
            }
        }

        private static async Task WriteErrorAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail(message);

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
