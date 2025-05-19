using Polly.CircuitBreaker;
using System.Net;

namespace TechChallenge.Cadastro.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BrokenCircuitException)
            {
                // Circuit Breaker aberto
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests; // 429
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "Serviço temporariamente indisponível devido a alta carga. Tente novamente mais tarde."
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                // Outros erros genéricos
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "Ocorreu um erro interno no servidor.",
                    detail = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}