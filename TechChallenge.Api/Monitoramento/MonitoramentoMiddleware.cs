using Prometheus;

namespace TechChallenge.Api.Monitoramento
{
    public class MonitoramentoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Counter _requestCounter;
        private readonly SystemMetricsCollector _metricsCollector;

        public MonitoramentoMiddleware(RequestDelegate next, SystemMetricsCollector metricsCollector)
        {
            _next = next;
            _metricsCollector = metricsCollector;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // Após o processamento da requisição, captura o código de status da resposta e o método HTTP
            var statusCode = context.Response.StatusCode.ToString();
            var method = context.Request.Method;

            // Incrementa o contador de requisições
            _metricsCollector.IncrementRequestCounter(method, statusCode);
        }
    }
}