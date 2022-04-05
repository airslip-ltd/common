using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using Airslip.Integrations.Banking.Types.Enums;
using System;
using System.Collections.Generic;

namespace Airslip.Integrations.Banking.Types.Models;

public record BankingBankModel : IModel
{
    public string? Id { get; set; }
    public EntityStatus EntityStatus { get; set; }
    public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    public string TradingName { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public EnvironmentType EnvironmentType { get; set; }
    public ICollection<string> CountryCodes { get; set; } = new List<string> {"GB"};
}