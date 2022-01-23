using Airslip.Common.Types.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.MerchantTransactions
{
    public interface IMerchantIntegrationService<TSource>
        where TSource : class
    {
        Task<ICollection<TrackingDetails>> SendBulk(
            IEnumerable<TSource> transactions,
            string entityId,
            AirslipUserType airslipUserType,
            string userId,
            string adapterSource);

        Task<TrackingDetails> Send(TSource transaction,
            string entityId,
            AirslipUserType airslipUserType,
            string userId,
            string adapterSource);
    }
}