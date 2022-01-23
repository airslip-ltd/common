using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.MerchantTransactions
{
    public class MerchantIntegrationService : IMerchantIntegrationService
    {
        private readonly IGeneratedRetailerApiV1Client _generatedRetailerApiV1Client;
        private readonly IMapper _mapper;

        public MerchantIntegrationService(IGeneratedRetailerApiV1Client generatedRetailerApiV1Client, IMapper mapper)
        {

            _generatedRetailerApiV1Client = generatedRetailerApiV1Client;
            _mapper = mapper;
        }

        public async Task<ICollection<TrackingDetails>> SendBulk<T>(
            IEnumerable<T> transactions,
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            string adapterSource) where T : class
        {
            List<TrackingDetails> trackingDetails = new();

            foreach (T transaction in transactions)
                trackingDetails.Add(await _send(transaction, entityId, airslipUserType, userId, adapterSource));

            return trackingDetails;
        }

        public Task<TrackingDetails> Send<T>(T transaction,
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            string adapterSource) where T : class
        {
            return _send(transaction, entityId, airslipUserType, userId, adapterSource);
        }

        private Task<TrackingDetails> _send<T>(T transaction, 
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            string adapterSource) where T : class
        {
            TransactionDetails transactionOut = _mapper.Map<TransactionDetails>(
                transaction,
                options => options.AfterMap((_, destinationRequest) =>
                {
                    destinationRequest.InternalId = CommonFunctions.GetId();
                    destinationRequest.Source = adapterSource;
                }));

            return _generatedRetailerApiV1Client
                .CreateTransactionAsync(entityId, airslipUserType.ToString(), userId, transactionOut);
        }
    }
}