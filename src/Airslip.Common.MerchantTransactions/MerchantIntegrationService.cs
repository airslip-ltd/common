using Airslip.Common.Types;
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

        public async Task<int> SendBulk<T>(IEnumerable<T> transactions, string airslipApiKey, string adapterSource) where T : class
        {
            _generatedRetailerApiV1Client.SetApiKeyToken(airslipApiKey);
            
            int count = 0;

            foreach (T transaction in transactions)
            {
                await _send(transaction, adapterSource);
                count++;
            }

            return count;
        }
        
        public async Task Send<T>(T transaction, string airslipApiKey, string adapterSource) where T : class
        {
            _generatedRetailerApiV1Client.SetApiKeyToken(airslipApiKey);

            await _send(transaction, adapterSource);
        }
        
        private async Task _send<T>(T transaction, string adapterSource) where T : class
        {
            Transaction transactionOut = _mapper.Map<Transaction>(
                transaction,
                options => options.AfterMap((_, destinationRequest) =>
                {
                    destinationRequest.InternalId = CommonFunctions.GetId();
                    destinationRequest.Source = adapterSource;
                }));

            await _generatedRetailerApiV1Client.CreateTransactionAsync(transactionOut);
        }
    }
}