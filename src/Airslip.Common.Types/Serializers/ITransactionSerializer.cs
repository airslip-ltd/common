namespace Airslip.Common.Types.Serializers
{
    public interface ITransactionSerializer
    {
        public string? GetLastCardDigits(string? maskedPanNumber);
    }
}