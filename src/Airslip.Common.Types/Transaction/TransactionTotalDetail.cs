namespace Airslip.Common.Types.Transaction;

public class TransactionTotalDetail 
{
    public long? SubtotalExcludingTax { get; set; }
    public long? WrappingExcludingTax { get; set; }
    public long? ShippingExcludingTax { get; set; }
    public long? TotalDiscount { get; set; }
    public long? TotalTax { get; set; }
    public long? Total { get; set; }
    public long? TotalPaid { get; set; }
}