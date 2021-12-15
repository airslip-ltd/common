using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.Consent.Models;

namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IMerchantDetails : IModel
    {
        MerchantSummaryModel MerchantDetails { get; set; }
    }
}