using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Entities;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Implementations
{
    public class RegisterTransactionService : IRegisterDataService<Transaction, IncomingTransactionModel>
    {
        private readonly IRepository<Transaction, IncomingTransactionModel> _repository;

        public RegisterTransactionService( 
            IRepository<Transaction, IncomingTransactionModel> repository)
        {
            _repository = repository;
        }
        
        public async Task RegisterData(IncomingTransactionModel model, DataSources dataSource)
        {
            model.DataSource = dataSource;
            model.TimeStamp = DateTime.UtcNow.ToUnixTimeMilliseconds();
            
            if (model is IModelWithOwnership ownedModel)
            {
                await _repository.Upsert(model.Id ?? string.Empty, model, ownedModel.UserId);
            }
            else
            {
                await _repository.Upsert(model.Id ?? string.Empty, model);
            }
        }

        public async Task RegisterData(string message, DataSources dataSource)
        {
            // Turn to object
            IncomingTransactionModel incomingTransactionModel = Json
                .Deserialize<IncomingTransactionModel>(message);

            await RegisterData(incomingTransactionModel, dataSource);
        }
    }
}