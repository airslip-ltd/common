namespace Airslip.Common.Types.Interfaces
{
    public interface IParsingService<TType>
    {
        ParsingResult<TType> TryParse(string payload);
    }
}