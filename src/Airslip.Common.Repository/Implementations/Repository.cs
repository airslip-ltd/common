using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Exception;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Types.Interfaces;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations
{
    /// <summary>
    /// Generic repository implementation for common CRUD functions, uses a design pattern which doesn't directly
    /// expose database entities to APIs but hides them behind Model classes
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TModel">The model type</typeparam>
    public class Repository<TEntity, TModel> : IRepository<TEntity, TModel> 
        where TEntity : class, IEntity 
        where TModel : class, IModel
    {
        private readonly IContext _context;
        // private readonly IModelValidator<TModel> _validator;
        private readonly IModelMapper<TModel> _mapper;
        private readonly IRepositoryLifecycle<TEntity, TModel> _repositoryLifecycle;

        public Repository(IContext context, 
            // IModelValidator<TModel> validator, 
            IModelMapper<TModel> mapper, 
            IRepositoryLifecycle<TEntity, TModel> repositoryLifecycle)
        {
            _context = context;
            // _validator = validator;
            _mapper = mapper;
            _repositoryLifecycle = repositoryLifecycle;
        }
        
        /// <summary>
        /// Add an entry to the context
        /// </summary>
        /// <param name="model">The model to add</param>
        /// <param name="userId">Optional User Id overrides value held internally</param>
        /// <returns>A response model containing any validation results, and the new model if successfully created</returns>
        public async Task<RepositoryActionResultModel<TModel>> Add(TModel model, string? userId = null)
        {
            // Could add some validation to see if the user is allowed to create this type of entity
            //  as part of a rule based system...?
            RepositoryAction<TEntity, TModel> repositoryAction = 
                new(null, null, model, LifecycleStage.Create, userId);
            
            // Validate the incoming model against the registered validator
            IResponse? validationResult = await _executeValidation(_repositoryLifecycle.PreValidateModel, 
                repositoryAction);

            if (validationResult is FailedActionResultModel<TModel> failedModelValidation) 
                return failedModelValidation;
            
            RepositoryLifecycleResult<TModel> lifecycle;
            try
            {
                lifecycle =
                    await _processLifecycle(repositoryAction);
            }
            catch (RepositoryLifecycleException exc)
            {
                // If not, return a not found message
                return new FailedActionResultModel<TModel>
                (
                    exc.ErrorCode,
                    ResultType.FailedVerification
                );
            }
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            {
                CurrentVersion = lifecycle.CurrentModel
            };
        }

        private async Task<IResponse?> _executeValidation(
            Func<RepositoryAction<TEntity, TModel>, Task<ValidationResultModel>> validationFunction, 
            RepositoryAction<TEntity, TModel> repositoryAction)
        {
            ValidationResultModel validationResult = await validationFunction(repositoryAction);
            
            // Return a new result model if validation has failed
            if (!validationResult.IsValid)
            {
                return new FailedActionResultModel<TModel>
                (
                    ErrorCodes.ValidationFailed,
                    ResultType.FailedValidation,
                    repositoryAction.Model,
                    ValidationResult: validationResult
                );
            }

            return null;
        }
        
        
        /// <summary>
        /// Updates an existing entry in the context
        /// </summary>
        /// <param name="id">The id of the entry to be update, must match the id on the model</param>
        /// <param name="model">The model with updated data</param>
        /// <param name="userId">Optional User Id overrides value held internally</param>
        /// <returns>A response model containing any validation results with previous and current versions of the model if successfully updated</returns>
        public async Task<RepositoryActionResultModel<TModel>> Update(string id, TModel model, string? userId = null)
        {
            RepositoryAction<TEntity, TModel> repositoryAction = 
                new(id, null, model, LifecycleStage.Update, userId);
            
            // Validate the incoming model against the registered validator
            IResponse? validationResult = await _executeValidation(_repositoryLifecycle.PreValidateModel, repositoryAction);

            if (validationResult is FailedActionResultModel<TModel> failedModelValidation) 
                return failedModelValidation;
            
            // Now we load the current entity version
            repositoryAction.SetEntity(await _context.GetEntity<TEntity>(id));
            
            validationResult = await _executeValidation(_repositoryLifecycle.PreValidateEntity, repositoryAction);

            if (validationResult is FailedActionResultModel<TModel> failedEntityValidation) 
                return failedEntityValidation;
            
            RepositoryLifecycleResult<TModel> lifecycle;
            
            try
            {
                lifecycle =
                    await _processLifecycle(repositoryAction);
            }
            catch (RepositoryLifecycleException exc)
            {
                return new FailedActionResultModel<TModel>
                (
                    exc.ErrorCode,
                    ResultType.FailedVerification
                );
            }
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                PreviousVersion: lifecycle.PreviousModel,
                CurrentVersion: lifecycle.CurrentModel
            );
        }

        /// <summary>
        /// Creates or updates entry in the context
        /// </summary>
        /// <param name="id">The id of the entry to be update, must match the id on the model</param>
        /// <param name="model">The model with updated data</param>
        /// <param name="userId">Optional User Id overrides value held internally</param>
        /// <returns>A response model containing any validation results with previous and current versions of the model if successfully updated</returns>
        public async Task<RepositoryActionResultModel<TModel>> Upsert(string id, TModel model, string? userId = null)
        {
            // Could add some validation to see if the user is allowed to create this type of entity
            //  as part of a rule based system...?
            RepositoryAction<TEntity, TModel> repositoryAction = 
                new(id, null, model, LifecycleStage.Update, userId);
            
            // Validate the incoming model against the registered validator
            IResponse? validationResult = await _executeValidation(_repositoryLifecycle.PreValidateModel, repositoryAction);

            if (validationResult is FailedActionResultModel<TModel> failedModelValidation) 
                return failedModelValidation;
            
            // Now we load the current entity version
            repositoryAction.SetEntity(await _context.GetEntity<TEntity>(id));
            repositoryAction.SetLifecycle(repositoryAction.Entity == null ? LifecycleStage.Create : LifecycleStage.Update);
            
            RepositoryLifecycleResult<TModel> lifecycle;
            try
            {
                lifecycle =
                    await _processLifecycle(repositoryAction);
            }
            catch (RepositoryLifecycleException exc)
            {
                // If not, return a not found message
                return new FailedActionResultModel<TModel>
                (
                    exc.ErrorCode,
                    ResultType.FailedVerification
                );
            }
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                PreviousVersion: lifecycle.PreviousModel,
                CurrentVersion: lifecycle.CurrentModel
            );
        }

        /// <summary>
        /// Marks an existing entry as deleted
        /// </summary>
        /// <param name="id">The id to mark as deleted</param>
        /// <param name="userId">Optional User Id overrides value held internally</param>
        /// <returns>A response model containing any validation results with previous version of the model if successfully deleted</returns>
        public async Task<RepositoryActionResultModel<TModel>> Delete(string id, string? userId = null)
        {
            RepositoryAction<TEntity, TModel> repositoryAction = 
                new(id, null, null, LifecycleStage.Delete, userId);
            
            IResponse? validationResult = 
                await _executeValidation(_repositoryLifecycle.PreValidateModel, repositoryAction);

            if (validationResult is FailedActionResultModel<TModel> failedModelValidation) 
                return failedModelValidation;
            
            // Now we load the current entity version
            repositoryAction.SetEntity(await _context.GetEntity<TEntity>(id));
            
            validationResult = await _executeValidation(_repositoryLifecycle.PreValidateEntity, repositoryAction);

            if (validationResult is FailedActionResultModel<TModel> failedEntityValidation) 
                return failedEntityValidation;

            RepositoryLifecycleResult<TModel> lifecycle;
            try
            {
                lifecycle =
                    await _processLifecycle(repositoryAction);
            }
            catch (RepositoryLifecycleException exc)
            {
                // If not, return a not found message
                return new FailedActionResultModel<TModel>
                (
                    exc.ErrorCode,
                    ResultType.FailedVerification
                );
            }

            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                PreviousVersion: lifecycle.PreviousModel
            );
        }

        /// <summary>
        /// Lookup a particular entity by Id 
        /// </summary>
        /// <param name="id">The id of the entity to return</param>
        /// <returns>A response model containing the current version of the model if successfully found</returns>
        public async Task<RepositoryActionResultModel<TModel>> Get(string id)
        {
            // Could add some validation to see if the user is allowed to delete this type of entity
            //  as part of a rule based system...?
            RepositoryAction<TEntity, TModel> repositoryAction = 
                new(id, null, null, LifecycleStage.Get, null);
            
            IResponse? validationResult = 
                await _executeValidation(_repositoryLifecycle.PreValidateModel, repositoryAction);

            if (validationResult is FailedActionResultModel<TModel> failedModelValidation) 
                return failedModelValidation;
            
            repositoryAction
                .SetEntity(await _context.GetEntity<TEntity>(id));
            
            validationResult = await _executeValidation(_repositoryLifecycle.PreValidateEntity, repositoryAction);

            if (validationResult is FailedActionResultModel<TModel> failedEntityValidation) 
                return failedEntityValidation;
            
            // If we have a search formatter we can use it here to populate any additional data
            repositoryAction
                .SetEntity(_repositoryLifecycle.PostProcess(repositoryAction));
            repositoryAction
                .SetModel(_mapper.Create(repositoryAction.Entity));
            repositoryAction
                .SetModel(await _repositoryLifecycle.PostProcessModel(repositoryAction));
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                repositoryAction.Model
            );
        }

        private Task<RepositoryLifecycleResult<TModel>> _processLifecycle(RepositoryAction<TEntity, TModel> repositoryAction)
        {
            // Validate the request
            if (repositoryAction.Entity == null && repositoryAction.Model == null) 
                throw new ArgumentException("Invalid parameter combination");
            if (repositoryAction.Model == null && repositoryAction.LifecycleStage != LifecycleStage.Delete) 
                throw new ArgumentException("Invalid parameter combination");
            if (repositoryAction.Entity == null && repositoryAction.LifecycleStage != LifecycleStage.Create) 
                throw new ArgumentException("Invalid parameter combination");

            return _processLifecycleAsync(repositoryAction);
        }

        private async Task<RepositoryLifecycleResult<TModel>> _processLifecycleAsync
            (RepositoryAction<TEntity, TModel> repositoryAction)
        {
            // Create a representation as it is today
            TModel? previousModel = repositoryAction.LifecycleStage == LifecycleStage.Create ? null : 
                _mapper.Create(repositoryAction.Entity);

            // Update from the model
            if (repositoryAction.Model != null)
            {
                repositoryAction.SetEntity(repositoryAction.LifecycleStage == LifecycleStage.Create
                        ? _mapper.Create<TEntity>(repositoryAction.Model)
                        : _mapper.Update(repositoryAction.Model, repositoryAction.Entity));
            }

            // Lifecycle
            repositoryAction
                .SetEntity(_repositoryLifecycle.PreProcess(repositoryAction));
            
            // Update in the context
            await _context.UpsertEntity(repositoryAction.Entity!);
            
            repositoryAction
                .SetEntity(_repositoryLifecycle.PostProcess(repositoryAction));
            
            repositoryAction
                .SetModel(_mapper.Create(repositoryAction.Entity));
            
            // Lifecycle
            await _repositoryLifecycle.PostProcessModel(repositoryAction);

            return new RepositoryLifecycleResult<TModel>
            {
                CurrentModel = repositoryAction.Model,
                PreviousModel = previousModel
            };
        }
    }

    internal class RepositoryLifecycleResult<TModel> 
        where TModel : class, IModel
    {
        public TModel? PreviousModel { get; set; }
        public TModel? CurrentModel { get; set; }
    }
}