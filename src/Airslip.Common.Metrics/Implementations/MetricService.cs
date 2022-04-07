using Airslip.Common.Metrics.Configuration;
using Airslip.Common.Metrics.Enums;
using Airslip.Common.Metrics.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;
using System.Diagnostics;

namespace Airslip.Common.Metrics.Implementations;

public class MetricService : IMetricService
{
    private readonly ILogger _logger;
    private readonly Stopwatch _stopwatch;
    private readonly MetricSettings _settings;
    private string _activityName = string.Empty;

    public MetricService(ILogger logger, IOptions<MetricSettings> options)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
        _settings = options.Value ?? new MetricSettings();
    }
    
    public void LogMetric(string metricName, MetricType metricType)
    {
        if (!_settings.IncludeMetrics) return;
        _logger.Debug("{MetricTime}ms: {ActivityName} - {MetricName} - {MetricType}", _stopwatch.ElapsedMilliseconds, 
            _activityName, metricName, metricType.ToString());
    }

    public void StartActivity(string activityName)
    {
        _activityName = activityName;
        _stopwatch.Restart();
    }

    public void StopActivity()
    {
        _stopwatch.Stop();
        long elapsed = _stopwatch.ElapsedMilliseconds;
        if (elapsed > _settings.Threshold) 
            _logger.Error(
                "Execution time of {MetricTime}ms beyond threshold of {Threshold}ms executing activity {ActivityName}",
                _stopwatch.ElapsedMilliseconds, 
                _settings.Threshold, _activityName);
    }
}