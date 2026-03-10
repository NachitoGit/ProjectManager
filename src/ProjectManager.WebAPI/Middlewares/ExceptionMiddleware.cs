using FluentValidation;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProjectManager.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, IHostEnvironment env)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ha ocurrido una excepción no controlada: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex, env);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IHostEnvironment env)
        {
            context.Response.ContentType = "application/json";
            
            var statusCode = (int)HttpStatusCode.InternalServerError;
            object response;

            switch (exception)
            {
                case ValidationException validationEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        message = "Se produjeron errores de validación.",
                        errors = validationEx.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                    };
                    break;

                case NotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response = new { message = notFoundEx.Message };
                    break;

                case ForbiddenAccessException:
                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    response = new { message = "No tienes permisos para realizar esta acción." };
                    break;

                default:
                    var message = env.IsDevelopment() ? exception.Message : "Error interno del servidor.";
                    var details = env.IsDevelopment() ? exception.StackTrace : null;

                    response = new
                    {
                        message,
                        details
                    };
                    break;
            }

            context.Response.StatusCode = statusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
