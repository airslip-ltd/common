using Airslip.Common.Types.Enums;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Types.Transaction;

public record TransactionEnvelope
{
    public TransactionDetails Transaction { get; set; } = new();
    public AirslipUserType AirslipUserType { get; set; }
    public string AccountId { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string? ApiKey { get; set; }
    public string TrackingId { get; set; } = string.Empty;
    public long CreatedTimeStamp { get; init; }
    public string? UserId { get; set; }
    public bool ValidateApiKey { get; set; } = true;
}