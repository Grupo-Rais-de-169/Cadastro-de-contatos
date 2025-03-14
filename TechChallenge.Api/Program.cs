using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;
using Prometheus;
using TechChallenge.Api.Configuration;
using TechChallenge.Api.Monitoramento;

namespace TechChallenge.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Monitoramento
            using var server = new KestrelMetricServer(port: 1234);
            server.Start();
            builder.ConfigureServices();

            var app = builder.Build();

            var cache = app.Services.GetRequiredService<IMemoryCache>();
            cache.Set("key", "value", new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
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