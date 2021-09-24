using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Types
{
    public record EmailOutcome(
        bool Success,
        string? ErrorReason = null) : ISuccess;
}