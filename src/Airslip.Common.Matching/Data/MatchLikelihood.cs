using Airslip.Common.Matching.Enum;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Data
{
    public record MatchLikelihood(string MatchSource, MatchPerspective MatchPerspective, string SourceTrackingId,
        string WithTrackingId, int Score, List<MatchMetric> Metrics);
}