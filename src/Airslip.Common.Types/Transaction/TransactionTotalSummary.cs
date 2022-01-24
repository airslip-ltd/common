namespace Airslip.Common.Types.Transaction;

public class TransactionTotalSummary 
{
    public long? Total { get; set; }
    public long? Subtotal { get; set; }
    public long? Shipping { get; set; }
    public long? Tax { get; set; }
    public long? Discount { get; set; }
}