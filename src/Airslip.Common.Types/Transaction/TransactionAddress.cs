using JetBrains.Annotations;

namespace Airslip.Common.Types.Transaction;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TransactionAddress
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Postcode { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public TransactionCountry? Country { get; set; }
    public string? Company { get; set; }
    public string? Fax { get; set; }
    public string? Website { get; set; }
    public string? Gender { get; set; }
    public string? Region { get; set; }
    public bool? Default { get; set; }
}