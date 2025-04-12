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
                await HandleExceptionAsync(context, exception, StatusCodes.Status404NotFound);
            }
            catch (BadRequestException exception)
            {
                await HandleExceptionAsync(context, exception, StatusCodes.Status400BadRequest);
            }
            catch (UnauthorizedException exception)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await HandleExceptionAsync(context, exception, StatusCodes.Status401Unauthorized);
            }
            catch (ValidationRequestException exception)
            {
                await HandleExceptionAsync(context, exception, StatusCodes.Status400BadRequest);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception, StatusCodes.Status500InternalServerError);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            _logger.LogError("An error occurred: {Message}", exception.Message);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
