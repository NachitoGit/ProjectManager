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

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var statusCode = (int)HttpStatusCode.InternalServerError;
            object response;

            switch (exception)
            {
                // 1. Errores de Validación (400)
                case ValidationException validationEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    var cleanErrors = validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    response = new
                    {
                        message = "Se produjeron errores de validación.",
                        errors = cleanErrors
                    };
                    break;

                // 2. Errores de Recurso no encontrado (404)
                case NotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response = new { message = notFoundEx.Message };
                    break;

                // 3. Errores de Permisos (403)
                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    response = new { message = "No tienes permisos para realizar esta acción." };
                    break;

                // 4. Todo lo demás (500)
                default:
                    response = new
                    {
                        message = "Error interno del servidor.",
                        details = exception.Message // En producción, ocultar 'details'
                    };
                    break;
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
