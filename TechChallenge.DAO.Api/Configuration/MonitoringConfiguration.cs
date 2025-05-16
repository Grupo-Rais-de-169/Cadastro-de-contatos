using Prometheus;
using TechChallenge.DAO.Api.Monitoramento;

namespace TechChallenge.DAO.Api.Configuration
{
    public static class MonitoringConfiguration
    {

        public static IApplicationBuilder UseMonitoringConfiguration(this WebApplication app)
        {
            // Registra o coletor de métricas no container de DI
            var systemMetricsCollector = new SystemMetricsCollector();
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                var timer = new System.Timers.Timer(20000); // Atualiza a cada 20 segundos
                timer.Elapsed += (sender, args) => systemMetricsCollector.Collect();
                timer.AutoReset = true;
                timer.Start();
            });
            app.UseHttpMetrics();
            app.UseMetricServer();
            return app;
        }
    }
}
