using Airslip.Common.Types.Interfaces;
using Airslip.Common.Types.Matching.Data;
using Airslip.Common.Types.Transaction;
using System.Collections.Generic;

namespace Airslip.Common.Types.Matching.Response
{
    public record MatchResultResponse(string MatchType,
        string TransactionTrackingId,
        string MatchTrackingId,
        MatchLikelihood MatchLikelihood) : ISuccess
    {
        public string? UserId { get; set; } = string.Empty;
        public List<MatchMetadata> Metadata { get; } = new();
    }
}