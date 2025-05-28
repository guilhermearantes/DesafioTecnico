using System.Net;
using System.Text.Json;

namespace DesafioTecnicoObjective.Exceptions
{
    /// <summary>
    /// Middleware para tratamento global de exceções.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    SaldoInsuficienteException => (int)HttpStatusCode.NotFound,
                    ContaNotFoundException => (int)HttpStatusCode.NotFound,
                    InvalidOperationException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var result = JsonSerializer.Serialize(new { erro = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
