using ProductsService.Domain.Exceptions;
using System.Text.Json;

namespace ProductsService.API.Middlewares
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
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await HandleExceptionAsync(context, exception);
            }
            catch (BadRequestException exception)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await HandleExceptionAsync(context, exception);
            }
            catch (UnauthorizedException exception)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await HandleExceptionAsync(context, exception);
            }
            catch (ValidationRequestException exception)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await HandleValidationExceptionAsync(context, exception);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleValidationExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError("An error occurred: {Message}", exception.Message);

            context.Response.ContentType = "application/json";

            var messages = JsonSerializer.Deserialize<string[]>(exception.Message);

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Validation errors occurred.",
                Errors = messages,
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError("An error occurred: {Message}", exception.Message);

            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
