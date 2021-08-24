using System;

namespace Airslip.Common.Monitoring.Models
{
    public record HealthCheckResult(string Name, string Value, bool Ok, Exception Exception);
}