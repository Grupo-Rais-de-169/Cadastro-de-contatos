using Prometheus;
using System.Diagnostics;
using System.Management;

namespace TechChallenge.Api.Monitoramento
{
    public class SystemMetricsCollector
    {
        private readonly Gauge _totalMemoryGauge = Metrics
            .CreateGauge("server_memory_total_bytes", "Total de memória RAM do servidor.");

        private readonly Gauge _availableMemoryGauge = Metrics
            .CreateGauge("server_memory_available_bytes", "Memória RAM disponível no servidor.");

        private readonly Gauge _cpuUsageGauge = Metrics
            .CreateGauge("server_cpu_usage_percent", "Uso da CPU pelo sistema (%).");

        private readonly PerformanceCounter _cpuCounter;
        private readonly Counter _requestCounter = Metrics.CreateCounter("http_requests_by_status_custom",
                                                 "Contagem de requisições HTTP por código de status",
                                                  new[] { "method", "status_code" });


        public SystemMetricsCollector()
        {
            // Inicializa o contador de CPU apenas no Windows
            if (OperatingSystem.IsWindows())
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _cpuCounter.NextValue(); // Primeira leitura retorna 0, então descartamos
            }
        }

        public void IncrementRequestCounter(string method, string statusCode)
        {
            _requestCounter.Labels(method, statusCode).Inc();
        }

        public void Collect()
        {
            _totalMemoryGauge.Set(GetTotalMemory());
            _availableMemoryGauge.Set(GetAvailableMemory());
            _cpuUsageGauge.Set(GetCpuUsage());
        }

        private static long GetTotalMemory()
        {
            using var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
            foreach (var obj in searcher.Get())
            {
                return Convert.ToInt64(obj["TotalPhysicalMemory"]);
            }
            return 0;
        }

        private long GetAvailableMemory()
        {
            using var searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
            foreach (var obj in searcher.Get())
            {
                return Convert.ToInt64(obj["FreePhysicalMemory"]) * 1024; // Convertendo de KB para Bytes
            }
            return 0;
        }

        private double GetCpuUsage()
        {
            return _cpuCounter?.NextValue() ?? 0;
        }

    }
}