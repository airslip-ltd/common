using Airslip.Common.Matching.Data;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Types.Transaction;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Response
{
    public record MatchResult(string MatchType,
        string TransactionTrackingId,
        string MatchedTrackingId,
        string OriginalTrackingId,
        int Score,
        MatchLikelihood MatchLikelihood) : ISuccess
    {
        public string? UserId { get; set; } = string.Empty;
        public List<MatchMetadata> Metadata { get; } = new();
    }
}