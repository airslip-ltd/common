using Airslip.Common.Types;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IParsingService<TType>
    {
        ParsingResult<TType> TryParse(string payload);
    }
}