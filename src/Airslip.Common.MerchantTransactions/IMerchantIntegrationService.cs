using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.MerchantTransactions
{
    public interface IMerchantIntegrationService
    {
        Task<ICollection<TrackingDetails>> SendBulk<T>(
            IEnumerable<T> transactions,
            string entityId,
            AirslipUserType airslipUserType,
            string userId,
            string adapterSource) where T : class;

        Task<TrackingDetails> Send<T>(T transaction,
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            string adapterSource) where T : class;
    }
}