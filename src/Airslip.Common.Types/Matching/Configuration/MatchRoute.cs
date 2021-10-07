using Airslip.Common.Types.Matching.Enum;

namespace Airslip.Common.Types.Matching.Configuration
{
    public class MatchRoute
    {
        public Direction Direction { get; set; } = Direction.Request;
        public string MatchType { get; set; } = string.Empty;
        public string RouteTo { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
    }
}