using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Utilities.Extensions;
using JetBrains.Annotations;
using System;

namespace Airslip.Common.Services.Consent.Implementations
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IncomingTransactionModel : IModel, IOwnedModel, IFromDataSource
    {
        public string? Id { get; set; }
        public EntityStatus EntityStatus { get; set; }
        public string BankTransactionId { get; set; } = string.Empty;
        public string? TransactionHash { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string BankId { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public long? AuthorisedDate { get; set; }
        public long CapturedDate { get; set; }
        public decimal Amount { get; set; }
        public string? CurrencyCode { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? AddressLine { get; set; }
        public string? LastCardDigits { get; set; }
        public string? IsoFamilyCode { get; set; }
        public string? ProprietaryCode { get; set; }
        public string? TransactionIdentifier { get; set; }
        public string? Reference { get; set; }
        public string? EntityId { get; set; }
        public DataSources DataSource { get; set; } = DataSources.Unknown;
        public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    }
}