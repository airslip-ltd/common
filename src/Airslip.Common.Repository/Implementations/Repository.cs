using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Exception;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities;
using System;
using System.Collections.Generic;
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
        private readonly IModelValidator<TModel> _validator;
        private readonly IModelMapper<TModel> _mapper;
        private readonly IRepositoryLifecycle<TEntity, TModel> _repositoryLifecycle;

        public Repository(IContext context, 
            IModelValidator<TModel> validator, 
            IModelMapper<TModel> mapper, 
            IRepositoryLifecycle<TEntity, TModel> repositoryLifecycle)
        {
            _context = context;
            _validator = validator;
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
            
            // Validate the incoming model against the registered validator
            ValidationResultModel validationResult = await _validator.ValidateAdd(model);

            // Return a new result model if validation has failed
            if (!validationResult.IsValid)
            {
                return new FailedActionResultModel<TModel>
                (
                    ErrorCodes.ValidationFailed,
                    ResultType.FailedValidation,
                    model,
                    ValidationResult: validationResult
                );
            }
            
            RepositoryLifecycleResult<TModel> lifecycle;
            try
            {
                lifecycle =
                    await _processLifecycle(null, model, LifecycleStage.Delete, userId);
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

        /// <summary>
        /// Updates an existing entry in the context
        /// </summary>
        /// <param name="id">The id of the entry to be update, must match the id on the model</param>
        /// <param name="model">The model with updated data</param>
        /// <param name="userId">Optional User Id overrides value held internally</param>
        /// <returns>A response model containing any validation results with previous and current versions of the model if successfully updated</returns>
        public async Task<RepositoryActionResultModel<TModel>> Update(string id, TModel model, string? userId = null)
        {
            // Could add some validation to see if the user is allowed to create this type of entity
            //  as part of a rule based system...?
            
            // Validate the Id supplied against that of the model, bit of a crude check but could prevent some simple tampering
            if (!id.Equals(model.Id))
            {
                return new FailedActionResultModel<TModel>
                (
                    ErrorCodes.ValidationFailed,
                    ResultType.FailedVerification,
                    PreviousVersion: model
                );
            }
            
            // Validate the incoming model against the registered validator
            ValidationResultModel validationResult = await _validator.ValidateUpdate(model);

            // Return a new result model if validation has failed
            if (!validationResult.IsValid)
            {
                return new FailedActionResultModel<TModel>
                (
                    ErrorCodes.ValidationFailed,
                    ResultType.FailedValidation,
                    PreviousVersion: model,
                    ValidationResult: validationResult
                );
            }
            
            // Now we load the current entity version
            TEntity? currentEntity = await _context.GetEntity<TEntity>(id);
            
            // Check to see if the entity was found within the context
            if (currentEntity == null || currentEntity.EntityStatus == EntityStatus.Deleted)
            {
                // If not, return a not found message
                return new FailedActionResultModel<TModel>
                (
                    ErrorCodes.NotFound,
                    ResultType.NotFound
                );
            }

            RepositoryLifecycleResult<TModel> lifecycle;
            
            try
            {
                lifecycle =
                    await _processLifecycle(currentEntity, model, LifecycleStage.Update, userId);
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
            
            // Validate the Id supplied against that of the model, bit of a crude check but could prevent some simple tampering
            if (!id.Equals(model.Id))
            {
                return new FailedActionResultModel<TModel>
                (
                    ErrorCodes.ValidationFailed,
                    ResultType.FailedVerification,
                    PreviousVersion: model
                );
            }
            
            // Validate the incoming model against the registered validator
            ValidationResultModel validationResult = await _validator.ValidateUpdate(model);

            // Return a new result model if validation has failed
            if (!validationResult.IsValid)
            {
                return new FailedActionResultModel<TModel>
                (
                    ErrorCodes.ValidationFailed,
                    ResultType.FailedValidation,
                    PreviousVersion: model,
                    ValidationResult: validationResult
                );
            }
            
            // Now we load the current entity version
            TEntity? upsertEntity = await _context.GetEntity<TEntity>(id);
            LifecycleStage lifecycleStage = 
                upsertEntity == null ? LifecycleStage.Create : LifecycleStage.Update;

            RepositoryLifecycleResult<TModel> lifecycle;
            try
            {
                lifecycle =
                    await _processLifecycle(upsertEntity, model, lifecycleStage, userId);
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
            // Could add some validation to see if the user is allowed to delete this type of entity
            //  as part of a rule based system...?
            
            // Now we load the current entity version
            TEntity? currentEntity = await _context.GetEntity<TEntity>(id);
            
            // Check to see if the entity was found within the context
            if (currentEntity == null || currentEntity.EntityStatus == EntityStatus.Deleted)
            {
                // If not, return a not found message
                return new FailedActionResultModel<TModel>
                (
                    ErrorCodes.NotFound,
                    ResultType.NotFound
                );
            }

            RepositoryLifecycleResult<TModel> lifecycle;
            try
            {
                lifecycle =
                    await _processLifecycle(currentEntity, null, LifecycleStage.Delete, userId);
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
            
            // Now we load the current entity version
            TEntity? currentEntity = await _context.GetEntity<TEntity>(id);
            
            // Check to see if the entity was found within the context
            if (currentEntity == null || currentEntity.EntityStatus == EntityStatus.Deleted)
            {
                // If not, return a not found message
                return new FailedActionResultModel<TModel>
                (
                    ErrorCodes.NotFound,
                    ResultType.NotFound
                );
            }
            
            // If we have a search formatter we can use it here to populate any additional data
            currentEntity = _repositoryLifecycle.PostProcess(currentEntity, LifecycleStage.Get);
            
            // Map the entity
            TModel newModel = _mapper.Create(currentEntity);
            
            // If we have a search formatter we can use it here to populate any additional data
            newModel = await _repositoryLifecycle.PostProcessModel(newModel, LifecycleStage.Get);
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                newModel
            );
        }

        private async Task<RepositoryLifecycleResult<TModel>> _processLifecycle(TEntity? entity, TModel? model,
            LifecycleStage lifecycleStage, string? userId)
        {
            // Validate the request
            if (entity == null && model == null) 
                throw new ArgumentException("Invalid parameter combination");
            if (model == null && lifecycleStage != LifecycleStage.Delete) 
                throw new ArgumentException("Invalid parameter combination");
            if (entity == null && lifecycleStage != LifecycleStage.Create) 
                throw new ArgumentException("Invalid parameter combination");
            
            // Create a representation as it is today
            TModel? previousModel = lifecycleStage == LifecycleStage.Create ? null : _mapper.Create(entity);

            // Update from the model
            if (model != null)
            {
                entity = 
                    lifecycleStage == LifecycleStage.Create ? 
                        _mapper.Create<TEntity>(model) : 
                        _mapper.Update(model, entity);
            }
            
            // Lifecycle
            entity = _repositoryLifecycle.PreProcess(entity!, lifecycleStage, userId);
            
            // Update in the context
            await _context.UpsertEntity(entity);
            
            // Lifecycle
            entity = _repositoryLifecycle.PostProcess(entity, lifecycleStage, userId);
            
            // Build model based on created entity
            TModel currentModel = _mapper.Create(entity);
            
            // Lifecycle
            await _repositoryLifecycle.PostProcessModel(currentModel, lifecycleStage);

            return new RepositoryLifecycleResult<TModel>
            {
                CurrentModel = currentModel,
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