using Prometheus;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.Cadastro.Api.Configuration;
using TechChallenge.DAO.Api.Configuration;
using TechChallenge.DAO.Api.Monitoramento;

namespace TechChallenge.DAO.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Monitoramento
            using var server = new KestrelMetricServer(port: 1235);
            server.Start();

            builder.ConfigureServices();

            var app = builder.Build();

            app.UseMiddleware<MonitoramentoMiddleware>();
            app.UseMonitoringConfiguration();

            app.MapMetrics();
            app.UseHttpMetrics();
            app.UseMetricServer();

            app.ConfigureMiddleware();

            await app.RunAsync();
        }
    }
}