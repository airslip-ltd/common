using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.Consent.Models;
using Airslip.SmartReceipts.Api.Core.Models;

namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IMerchantDetails : IModel
    {
        TransactionMerchantModel Merchant { get; }
        MerchantSummaryModel? MerchantDetails { get; set; }
    }
}