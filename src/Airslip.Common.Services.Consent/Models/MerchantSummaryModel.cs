using Airslip.Common.Types.Enums;
using JetBrains.Annotations;

namespace Airslip.Common.Services.Consent.Models
{
    public record MerchantSummaryModel(
        string? Id,
        string? Name,
        string? CategoryCode,
        MerchantTypes Type)
    {
        public string? IconUrl { [UsedImplicitly] get; private set; }
        public string? LogoUrl { [UsedImplicitly] get; private set; }

        public void SetLogoUrl(string newUrl)
        {
            LogoUrl = newUrl;
        }
        
        public void SetIconUrl(string newUrl)
        {
            IconUrl = newUrl;
        }
    }
}