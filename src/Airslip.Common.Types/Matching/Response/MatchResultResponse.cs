using Airslip.Common.Types.Interfaces;
using Airslip.Common.Types.Matching.Data;

namespace Airslip.Common.Types.Matching.Response
{
    public record MatchResultResponse(string MatchType, string TransactionTrackingId, string MatchTrackingId, 
        MatchLikelihood MatchLikelihood) : ISuccess;
}