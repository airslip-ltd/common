using Airslip.Common.MerchantTransactions.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.MerchantTransactions.Implementations
{
    public class MerchantIntegrationService<TSource> : IMerchantIntegrationService<TSource> 
        where TSource : class
    {
        private readonly IGeneratedRetailerApiV1Client _generatedRetailerApiV1Client;
        private readonly ITransactionMapper<TSource> _transactionMapper;

        public MerchantIntegrationService(IGeneratedRetailerApiV1Client generatedRetailerApiV1Client, 
            ITransactionMapper<TSource> transactionMapper)
        {

            _generatedRetailerApiV1Client = generatedRetailerApiV1Client;
            _transactionMapper = transactionMapper;
        }

        public async Task<ICollection<TrackingDetails>> SendBulk(
            IEnumerable<TSource> transactions,
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            string adapterSource)
        {
            List<TrackingDetails> trackingDetails = new();

            foreach (TSource transaction in transactions)
                trackingDetails.Add(await _send(transaction, entityId, airslipUserType, userId, adapterSource));

            return trackingDetails;
        }

        public Task<TrackingDetails> Send(TSource transaction,
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            string adapterSource)
        {
            return _send(transaction, entityId, airslipUserType, userId, adapterSource);
        }

        private Task<TrackingDetails> _send(TSource transaction, 
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            string adapterSource)
        {
            TransactionDetails transactionOut = _transactionMapper.Create(
                transaction);
                
            transactionOut.InternalId = CommonFunctions.GetId();
            transactionOut.Source = adapterSource;

            return _generatedRetailerApiV1Client
                .CreateTransactionAsync(entityId, airslipUserType.ToString(), userId, transactionOut);
        }
    }
}