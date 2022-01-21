using JetBrains.Annotations;

namespace Airslip.Common.Types.Transaction;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TransactionCountry 
{
    public string? Code2 { get; set; }
    public string? Code3 { get; set; }
    public string? Name { get; set; }
}