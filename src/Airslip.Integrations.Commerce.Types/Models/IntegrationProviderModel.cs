using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace Airslip.Integrations.Commerce.Types.Models;

public record CommerceProviderModel : IModel, IFromDataSource
{
    public string? Id { get; set; }
    public EntityStatus EntityStatus { get; set; } = EntityStatus.Active;
    public EnvironmentType EnvironmentType { get; set; } = EnvironmentType.Live;
    public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    public List<IntegrationCountryCodeModel> CountryCodes { get; set; } = new();
    public DataSources DataSource { get; set; } = DataSources.Unknown;
    public string Name { get; set; } = String.Empty;
    public string Provider { get; set; } = string.Empty;
    public string FriendlyName { get; set; } = string.Empty;
    public IntegrationType IntegrationType { get; set; } = IntegrationType.Commerce;
    public string? Icon { get; set; }
    public string? Logo { get; set; }
    public string Integration { get; set; } = string.Empty;
    public int Priority { get; set; }
    public AvailabilityType Availability { get; set; } = AvailabilityType.ComingSoon;
    public int InstallationCount { get; set; }
}