using Airslip.Common.Contracts;

namespace Airslip.Common.Types
{
    public record EmailOutcome(
        bool Success,
        string? ErrorReason = null) : ISuccess;
}