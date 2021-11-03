using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Types;
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
        private readonly IModelValidator<TModel> _validator;
        private readonly IModelMapper<TModel> _mapper;
        private readonly IModelDeliveryService<TModel> _deliveryService;
        private readonly IRepositoryUserService _userService;

        public Repository(IContext context, IModelValidator<TModel> validator, 
            IModelMapper<TModel> mapper, IModelDeliveryService<TModel> deliveryService, 
            IRepositoryUserService userService)
        {
            _context = context;
            _validator = validator;
            _mapper = mapper;
            _deliveryService = deliveryService;
            _userService = userService;
        }
        
        public Repository(IContext context, IModelValidator<TModel> validator, 
            IModelMapper<TModel> mapper,  
            IRepositoryUserService userService) : this(context, validator, mapper, 
            new NullModelDeliveryService<TModel>(), userService)
        {
            
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
            
            // If passed, assume all ok and create a new entity
            TEntity newEntity = _mapper.Create<TEntity>(model);

            // Assign a few defaults, guid and who created it
            newEntity.Id = string.IsNullOrWhiteSpace(newEntity.Id) ? 
                CommonFunctions.GetId() : newEntity.Id;
            newEntity.EntityStatus = EntityStatus.Active;
            newEntity.AuditInformation = new BasicAuditInformation
            {
                DateCreated = DateTime.UtcNow,
                CreatedByUserId = userId ?? _userService.UserId
            };
            
            // Add the entity
            await _context.AddEntity(newEntity);
            
            // Build model based on created entity
            TModel newModel = _mapper.Create(newEntity);
            
            // Deliver model using delivery service
            await _deliveryService.Deliver(newModel);
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                newModel
            );
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
            
            // Create a representation as it is today
            var currentModel = _mapper.Create(currentEntity);
            
            // Update the current entity with the new values passed in
            _mapper.Update(model, currentEntity);
            
            // Assign the defaults for updated flags
            currentEntity.AuditInformation ??= new BasicAuditInformation();
            currentEntity.AuditInformation.DateUpdated = DateTime.UtcNow;
            currentEntity.AuditInformation.UpdatedByUserId = userId ?? _userService.UserId;
            
            // Update in the context
            await _context.UpdateEntity(currentEntity);
            
            // Build model based on created entity
            TModel newModel = _mapper.Create(currentEntity);
            
            // Deliver model using delivery service
            await _deliveryService.Deliver(newModel);
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                PreviousVersion: currentModel,
                CurrentVersion: newModel
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
            TModel? currentModel, previousModel = null;

            // Check to see if the entity was found within the context
            if (upsertEntity == null || upsertEntity.EntityStatus == EntityStatus.Deleted)
            {
                // If passed, assume all ok and create a new entity
                upsertEntity = _mapper.Create<TEntity>(model);

                // Assign a few defaults, guid and who created it
                upsertEntity.Id = string.IsNullOrWhiteSpace(upsertEntity.Id) ? 
                    CommonFunctions.GetId() : upsertEntity.Id;
                upsertEntity.EntityStatus = EntityStatus.Active;
                upsertEntity.AuditInformation = new BasicAuditInformation
                {
                    DateCreated = DateTime.UtcNow,
                    CreatedByUserId = userId ?? _userService.UserId
                };
                
                // Create a representation as it is today
                currentModel = _mapper.Create(upsertEntity);
            }
            else
            {
                // Create a representation as it is today
                previousModel = _mapper.Create(upsertEntity);
                
                // Update the current entity with the new values passed in
                _mapper.Update(model, upsertEntity);
            
                // Assign the defaults for updated flags
                upsertEntity.AuditInformation ??= new BasicAuditInformation();
                upsertEntity.AuditInformation.DateUpdated = DateTime.UtcNow;
                upsertEntity.AuditInformation.UpdatedByUserId = userId ?? _userService.UserId;     
                
                // Create a representation as it is today
                currentModel = _mapper.Create(upsertEntity);
            }
            
            // Update in the context
            await _context.UpsertEntity(upsertEntity);
            
            // Deliver model using delivery service
            await _deliveryService.Deliver(currentModel);
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                PreviousVersion: previousModel,
                CurrentVersion: currentModel
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
                        
            // Create a representation as it is today
            TModel currentModel = _mapper.Create(currentEntity);
            
            // Assign the defaults for updated flags
            currentEntity.AuditInformation ??= new BasicAuditInformation();
            currentEntity.AuditInformation.DateDeleted = DateTime.UtcNow;
            currentEntity.AuditInformation.DeletedByUserId = userId ?? _userService.UserId;
            currentEntity.EntityStatus = EntityStatus.Deleted;

            // Update in the context
            await _context.UpdateEntity(currentEntity);
            
            // Build model based on created entity
            TModel newModel = _mapper.Create(currentEntity);
            
            // Deliver model using delivery service
            await _deliveryService.Deliver(newModel);
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                PreviousVersion: currentModel
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
            
            // Create a result containing old and new version, and return
            return new SuccessfulActionResultModel<TModel>
            (
                _mapper.Create(currentEntity)
            );
        }
    }
}