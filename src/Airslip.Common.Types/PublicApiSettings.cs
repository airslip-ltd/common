using JetBrains.Annotations;

namespace Airslip.Common.Types
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PublicApiSettings
    {
        public string BaseUri { get; init; } = string.Empty;
    }
}