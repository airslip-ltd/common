using Airslip.Common.Types.Matching.Enum;
using System.Collections.Generic;

namespace Airslip.Common.Types.Matching.Data
{
    public record MatchLikelihood(string MatchSource, MatchPerspective MatchPerspective, string SourceTrackingId,
        string WithTrackingId, int Score, List<MatchMetric> Metrics);
}