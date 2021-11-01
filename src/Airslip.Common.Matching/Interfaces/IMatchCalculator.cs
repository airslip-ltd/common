using Airslip.Common.Types.Matching.Data;
using Airslip.Common.Types.Matching.Enum;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IMatchCalculator
    {
        MatchLikelihood CalculateMatchLikelihood(MatchPerspective matchPerspective, IMatchable source, IMatchable with);
    }
}