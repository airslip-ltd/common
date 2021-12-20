using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Implementations
{
    public class RegisterDataService<TEntity, TModel> : IRegisterDataService<TEntity, TModel>
        where TModel : class, IModel, IFromDataSource
        where TEntity : class, IEntity, IFromDataSource
    {
        private readonly ILogger _logger;
        private readonly IRepository<TEntity, TModel> _repository;

        public RegisterDataService(
            ILogger logger,
            IRepository<TEntity, TModel> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task RegisterData(TModel model, DataSources dataSource)
        {
            try
            {
                model.DataSource = dataSource;
                model.TimeStamp = DateTime.UtcNow.ToUnixTimeMilliseconds();
                
                string id = model.Id ?? string.Empty;
                if (model is IModelWithOwnership ownedModel)
                    await _repository.Upsert(id, model, ownedModel.UserId);
                else
                    await _repository.Upsert(id, model);
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "Unhandled exception for the data source {DataSource} with data packet {Model}", dataSource, model);
            }
        }

        public Task RegisterData(string message, DataSources dataSource)
        {
            // Turn to object
            TModel bankTransaction = Json.Deserialize<TModel>(message);

            return RegisterData( bankTransaction, dataSource);
        }
    }
}