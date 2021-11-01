using Airslip.Common.Matching.Data;
using Airslip.Common.Matching.Enum;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IMatchCalculator
    {
        MatchLikelihood CalculateMatchLikelihood(MatchPerspective matchPerspective, IMatchable source, IMatchable with);
    }
}