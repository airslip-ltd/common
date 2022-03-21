using Airslip.Common.Repository.Configuration;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;
using System.Diagnostics;

namespace Airslip.Common.Repository.Implementations;

public class RepositoryLogService : IRepositoryLogService
{
    private readonly ILogger _logger;
    private readonly Stopwatch _stopwatch;
    private readonly RepositorySettings _settings;

    public RepositoryLogService(ILogger logger, IOptions<RepositorySettings> options)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
        _settings = options.Value ?? new RepositorySettings();
    }
    
    public void LogMetric(string activityName, string metricName, MetricType metricType)
    {
        if (!_settings.IncludeMetrics) return;
        _logger.Debug("{MetricTime}ms: {ActivityName} - {MetricName} - {MetricType}", _stopwatch.ElapsedMilliseconds, 
            activityName, metricName, metricType.ToString());
    }

    public void StartClock()
    {
        if (!_settings.IncludeMetrics) return;
        _stopwatch.Restart();
    }

    public void StopClock()
    {
        if (!_settings.IncludeMetrics) return;
        _stopwatch.Stop();
    }
}