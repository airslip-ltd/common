using Airslip.Common.Auth.Functions.Configuration;
using Airslip.Common.Types.Enums;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Airslip.Common.Auth.Functions.Data;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class ApiAccessRights
{
    internal static List<ApiAccessDefinition> AccessDefinitions { get; } = new();

    // Legacy support for existing settings...
    internal static void AddFromSettings(IConfiguration configuration)
    {
        IConfigurationSection? configurationSection = configuration
            .GetSection(nameof(ApiAccessSettings));

        ApiAccessSettings? apiSettings = configurationSection?
            .Get<ApiAccessSettings>();
            
        if (apiSettings != null) 
            AccessDefinitions
                .Add(new ApiAccessDefinition(apiSettings.AllowedTypes, apiSettings.AllowedEntities));
    }

    internal static void AddNamedAccessRights(string named, List<AirslipUserType> allowedTypes,
        List<string> allowedEntities)
    {
        AccessDefinitions.Add(new ApiAccessDefinition(allowedTypes, allowedEntities, named));
    }

    internal static void AddGeneralAccessRights(List<AirslipUserType> allowedTypes, List<string> allowedEntities)
    {
        AccessDefinitions.Add(new ApiAccessDefinition(allowedTypes, allowedEntities));
    }
}