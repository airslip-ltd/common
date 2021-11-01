using Airslip.Common.Matching.Data;
using Airslip.Common.Matching.Enum;
using Airslip.Common.Types.Transaction;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Models
{
    public record MatchResultModel(string MatchType,
        string TransactionTrackingId,
        string MatchTrackingId,
        string UserId,
        MatchLikelihood MatchLikelihood)
    {
        public List<MatchMetadata> Metadata { get; } = new();
    }
    
    public record AttemptMatchRequest(MatchPerspective Perspective, string TrackingId);
}