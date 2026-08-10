using Aplicacion.Exceptions;

namespace Daily_food.Middleware
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
                var (status, message) = ex switch
                {
                    NotFoundException e => (StatusCodes.Status404NotFound, e.Message),
                    UnauthorizedException e => (StatusCodes.Status401Unauthorized, e.Message),
                    BusinessException e => (StatusCodes.Status400BadRequest, e.Message),
                    _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor.")
                };

                context.Response.StatusCode = status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = message });
            }
        }
    }
}
