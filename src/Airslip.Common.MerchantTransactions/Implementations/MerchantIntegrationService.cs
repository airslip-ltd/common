using Airslip.Common.MerchantTransactions.Generated;
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
        private readonly IInternalApiV1Client _internalApiV1Client;
        private readonly IExternalApiV1Client _externalApiV1Client;
        private readonly ITransactionMapper<TSource> _transactionMapper;

        public MerchantIntegrationService(IInternalApiV1Client internalApiV1Client, 
            IExternalApiV1Client externalApiV1Client,
            ITransactionMapper<TSource> transactionMapper)
        {
            _internalApiV1Client = internalApiV1Client;
            _externalApiV1Client = externalApiV1Client;
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
                trackingDetails.Add(await _sendInternalApi(transaction, entityId, airslipUserType, userId, adapterSource));

            return trackingDetails;
        }

        public Task<TrackingDetails> Send(TSource transaction,
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            string adapterSource)
        {
            return _sendInternalApi(transaction, entityId, airslipUserType, userId, adapterSource);
        }

        public async Task<ICollection<TrackingDetails>> SendBulk(IEnumerable<TSource> transactions, string airslipApiKey, string adapterSource)
        {
            List<TrackingDetails> trackingDetails = new();

            foreach (TSource transaction in transactions)
                trackingDetails.Add(await _sendExternalApi(transaction, airslipApiKey, adapterSource));

            return trackingDetails;
        }

        public Task<TrackingDetails> Send(TSource transaction, string airslipApiKey, string adapterSource)
        {
            return _sendExternalApi(transaction, airslipApiKey, adapterSource);
        }

        private Task<TrackingDetails> _sendInternalApi(TSource transaction, 
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            string adapterSource)
        {
            TransactionDetails transactionOut = _transactionMapper.Create(
                transaction);
                
            transactionOut.InternalId = CommonFunctions.GetId();
            transactionOut.Source = adapterSource;

            return _internalApiV1Client
                .CreateTransactionAsync(entityId, airslipUserType.ToString(), userId, transactionOut);
        }
        
        private Task<TrackingDetails> _sendExternalApi(TSource transaction, string apiKeyToken, 
            string adapterSource)
        {
            TransactionDetails transactionOut = _transactionMapper.Create(
                transaction);
                
            transactionOut.InternalId = CommonFunctions.GetId();
            transactionOut.Source = adapterSource;

            _externalApiV1Client.SetApiKeyToken(apiKeyToken);
            return _externalApiV1Client.CreateTransactionAsync(transactionOut);
        }
    }
}