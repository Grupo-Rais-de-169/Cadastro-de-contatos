using Prometheus;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Management;

namespace TechChallenge.DAO.Api.Monitoramento
{
    [ExcludeFromCodeCoverage]
    public class SystemMetricsCollector
    {
        private readonly Gauge _totalMemoryGauge = Metrics
            .CreateGauge("server_memory_total_bytes", "Total de memória RAM do servidor.");

        private readonly Gauge _availableMemoryGauge = Metrics
            .CreateGauge("server_memory_available_bytes", "Memória RAM disponível no servidor.");

        private readonly Gauge _cpuUsageGauge = Metrics
            .CreateGauge("server_cpu_usage_percent", "Uso da CPU pelo sistema (%).");

        private readonly PerformanceCounter? _cpuCounter;
        private readonly Counter _requestCounter = Metrics.CreateCounter("http_requests_by_status_custom",
                                                 "Contagem de requisições HTTP por código de status",
                                                  "method", "status_code");

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
            if (OperatingSystem.IsWindows())
            {
                using var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                foreach (var obj in searcher.Get())
                {
                    return Convert.ToInt64(obj["TotalPhysicalMemory"]);
                }
            }
            else if (OperatingSystem.IsLinux())
            {
                return GetLinuxMemoryInfo("MemTotal");
            }
            else if (OperatingSystem.IsMacOS())
            {
                return GetMacMemoryInfo();
            }
            return 0;
        }

        private static long GetAvailableMemory()
        {
            if (OperatingSystem.IsWindows())
            {
                using var searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
                var result = searcher.Get().Cast<ManagementObject>().FirstOrDefault();
                return result != null ? Convert.ToInt64(result["FreePhysicalMemory"]) * 1024 : 0;
            }

            if (OperatingSystem.IsLinux())
            {
                return GetLinuxMemoryInfo("MemAvailable");
            }

            if (OperatingSystem.IsMacOS())
            {
                return GetMacMemoryInfo();
            }

            return 0;
        }

        private double GetCpuUsage()
        {
            if (OperatingSystem.IsWindows())
            {
                return _cpuCounter?.NextValue() ?? 0;
            }
            return 0; // Retorna 0 para outros sistemas operacionais
        }

        // Para Linux
        private static long GetLinuxMemoryInfo(string key)
        {
            var line = File.ReadLines("/proc/meminfo")
                           .FirstOrDefault(x => x.StartsWith(key));

            if (line != null)
            {
                var parts = line.Split(':');
                if (parts.Length > 1)
                {
                    return Convert.ToInt64(parts[1].Trim().Split(' ')[0]) * 1024;
                }
            }

            return 0;
        }

        // Para macOS
        private static long GetMacMemoryInfo()
        {
            var output = ExecuteCommand("sysctl hw.memsize");
            var parts = output.Split(':');
            return parts.Length > 1 ? Convert.ToInt64(parts[1].Trim()) : 0;
        }

        private static string ExecuteCommand(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            return process.StandardOutput.ReadToEnd().Trim();
        }

    }
}