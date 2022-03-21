using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations;

namespace Airslip.Common.Repository.Interfaces;

public interface IRepositoryLogService
{
    void LogMetric(string activityName, string metricName, MetricType metricType);
    void StartClock();
    void StopClock();
}