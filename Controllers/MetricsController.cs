using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace lab18.Controllers;

[ApiController]
[Route("[controller]")]
public class MetricsController : ControllerBase
{
    private static readonly Counter RequestCounter = Metrics
        .CreateCounter("app_requests_total", "Количество запросов");
    
    private static readonly Gauge CpuUsage = Metrics
        .CreateGauge("app_cpu_usage", "Занятость процессора");
    
    private static readonly Histogram RequestDuration = Metrics
        .CreateHistogram("app_request_duration_seconds", "Длительность запроса",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.01, 2, 10)
            });

    private readonly Random _random = new Random();

    [HttpGet("test")]
    public IActionResult Test()
    {
        RequestCounter.Inc();

        CpuUsage.Set(_random.NextDouble() * 100);

        using (RequestDuration.NewTimer())
        {
            Thread.Sleep(_random.Next(10, 100));
        }

        return Ok(new { message = "Test request processed" });
    }
}
