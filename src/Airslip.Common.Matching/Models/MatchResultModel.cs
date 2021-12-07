using Airslip.Common.Matching.Data;
using Airslip.Common.Matching.Enum;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Transaction;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Models
{
    public record MatchResultModel(MatchTypes MatchType,
        string TransactionTrackingId,
        string MatchTrackingId,
        string? UserId,
        string? EntityId,
        AirslipUserType AirslipUserType,
        MatchLikelihood MatchLikelihood)
    {
        public List<MatchMetadata> Metadata { get; } = new();
    }
    
    public record AttemptMatchRequest(MatchPerspective Perspective, string TrackingId);
}