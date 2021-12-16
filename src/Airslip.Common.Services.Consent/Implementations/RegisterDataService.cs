using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Implementations
{
    public class RegisterDataService<TEntity, TModel> : IRegisterDataService<TEntity, TModel> 
        where TModel : class, IModel, IFromDataSource
        where TEntity : class, IEntity, IFromDataSource
    {
        private readonly IRepository<TEntity, TModel> _repository;

        public RegisterDataService( 
            IRepository<TEntity, TModel> repository)
        {
            _repository = repository;
        }
        
        public async Task RegisterData(TModel model, DataSources dataSource)
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
            var bankTransaction = Json.Deserialize<TModel>(message);

            await RegisterData(bankTransaction, dataSource);
        }
    }
}