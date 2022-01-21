namespace Airslip.Common.Types.Transaction;

public class TransactionTotalSummary 
{
    public double? Total { get; set; }
    public double? Subtotal { get; set; }
    public double? Shipping { get; set; }
    public double? Tax { get; set; }
    public double? Discount { get; set; }
}