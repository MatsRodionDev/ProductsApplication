using OrderService.Domain.Exceptions;
using System.Text.Json;

namespace OrderService.API.Middlewares
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
            catch (NotFoundException exception)
            { 
                await HandleExceptionAsync(context, exception, StatusCodes.Status404NotFound);
            }
            catch (BadRequestException exception)
            {
                await HandleExceptionAsync(context, exception, StatusCodes.Status400BadRequest);
            }
            catch (UnauthorizedException exception)
            {
                await HandleExceptionAsync(context, exception, StatusCodes.Status401Unauthorized);
            }
            catch (ValidationRequestException exception)
            {
                await HandleValidationExceptionAsync(context, exception, StatusCodes.Status400BadRequest);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception, StatusCodes.Status500InternalServerError);
            }
        }

        private Task HandleValidationExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            _logger.LogError("An error occurred: {Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var messages = JsonSerializer.Deserialize<string[]>(exception.Message);

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Validation errors occurred.",
                Errors = messages,
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            _logger.LogError("An error occurred: {Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
