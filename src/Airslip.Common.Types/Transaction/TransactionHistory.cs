namespace Airslip.Common.Types.Transaction;

public class TransactionHistory 
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IntegrationDateTime? ModifiedTime { get; set; }
    public bool? Notify { get; set; }
    public string? Comment { get; set; }
}