using Airslip.Common.Metrics.Enums;

namespace Airslip.Common.Metrics.Interfaces;

public interface IMetricService
{
    void LogMetric(string metricName, MetricType metricType);
    void StartActivity(string activityName);
    void StopActivity();
}