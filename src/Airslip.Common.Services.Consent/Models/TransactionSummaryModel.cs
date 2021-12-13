using Airslip.Common.Matching.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Hateoas;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Airslip.Common.Services.Consent.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TransactionSummaryModel : LinkResourceBase, IMerchantDetails
    {
        public string? Id { get; set; } = string.Empty;
        public EntityStatus EntityStatus { get; set; }
        public MatchTypes MatchType { get; set; } = MatchTypes.Unknown;
        public string OriginalTrackingId { get; set; } = string.Empty;
        public string? MatchedTrackingId { get; set; }
        public string? AuthorisedTimeStamp { get; set; }
        public string? CapturedTimeStamp { get; set; }
        public string? Amount { get; set; }
        public string? CurrencyCode { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public TransactionMerchantModel Merchant { get; init; } = new();
        [JsonIgnore]
        public TransactionBankModel? BankDetails { get; set; }
        public MerchantSummaryModel? MerchantDetails { get; set; }
        public string? AccountId { get; set; }
    }
}