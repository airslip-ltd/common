using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.MerchantTransactions
{
    public interface IMerchantIntegrationService
    {
        Task<int> SendBulk<T>(IEnumerable<T> transactions, string airslipApiKey, string adapterSource) where T : class;
        Task Send<T>(T transaction, string airslipApiKey, string adapterSource) where T : class;
    }
}