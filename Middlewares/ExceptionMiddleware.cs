using CurrencyExchangeService.Exceptions;
using System.Net;

namespace CurrencyExchangeService.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An error occurred");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                InvalidCurrencyException => (int)HttpStatusCode.BadRequest,
                ApiException => (int)HttpStatusCode.ServiceUnavailable,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            var response = new
            {
              type = exception.GetType().Name,
              message = exception.Message,
              statusCode = context.Response.StatusCode,
              timestamp = DateTime.UtcNow,
              path = context.Request.Path.ToString()
            };
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}