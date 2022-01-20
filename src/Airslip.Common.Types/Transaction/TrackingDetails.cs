using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Types.Transaction
{
    public record TrackingDetails(string TrackingId) : ISuccess;
}