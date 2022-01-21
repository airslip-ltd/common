namespace Airslip.Common.Types.Transaction;

public record TransactionDiscount
{
    /// <summary>
    /// The name of the discount. 
    /// </summary>
    /// <example>Voucher</example>
    public string? Name { get; init; }
        
    /// <summary>
    /// The discounted amount.
    /// </summary>
    /// <example>1846</example>
    //[Required]
    public long? Amount { get; init; }
}