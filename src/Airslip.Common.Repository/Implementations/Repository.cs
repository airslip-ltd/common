using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
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
        private readonly UserToken _userToken;

        public Repository(IContext context, IModelValidator<TModel> validator, IModelMapper<TModel> mapper, 
            ITokenService<UserToken, GenerateUserToken> tokenService)
        {
            _context = context;
            _validator = validator;
            _mapper = mapper;
            _userToken = tokenService.GetCurrentToken();
        }
        
        /// <summary>
        /// Add an entry to the context
        /// </summary>
        /// <param name="model">The model to add</param>
        /// <returns>A response model containing any validation results, and the new model if successfully created</returns>
        public async Task<RepositoryActionResultModel<TModel>> Add(TModel model)
        {
            // Could add some validation to see if the user is allowed to create this type of entity
            //  as part of a rule based system...?
            
            // Validate the incoming model against the registered validator
            ValidationResultModel validationResult = await _validator.ValidateAdd(model);

            // Return a new result model if validation has failed
            if (!validationResult.IsValid)
            {
                return new RepositoryActionResultModel<TModel>
                (
                    ResultType.FailedValidation,
                    model,
                    ValidationResult: validationResult
                );
            }
            
            // If passed, assume all ok and create a new entity
            TEntity newEntity = _mapper.Create<TEntity>(model);

            // Assign a few defaults, guid and who created it
            newEntity.Id = CommonFunctions.GetId();
            newEntity.EntityStatus = EntityStatus.Active;
            newEntity.AuditInformation = new BasicAuditInformation
            {
                DateCreated = DateTime.UtcNow,
                CreatedByUserId = _userToken.UserId
            };
            
            // Add the entity
            await _context.AddEntity(newEntity);
            
            // Create a result containing old and new version, and return
            return new RepositoryActionResultModel<TModel>
            (
                ResultType.Success,
                _mapper.Create(newEntity)
            );
        }

        /// <summary>
        /// Updates an existing entry in the context
        /// </summary>
        /// <param name="id">The id of the entry to be update, must match the id on the model</param>
        /// <param name="model">The model with updated data</param>
        /// <returns>A response model containing any validation results with previous and current versions of the model if successfully updated</returns>
        public async Task<RepositoryActionResultModel<TModel>> Update(string id, TModel model)
        {
            // Could add some validation to see if the user is allowed to create this type of entity
            //  as part of a rule based system...?
            
            // Validate the Id supplied against that of the model, bit of a crude check but could prevent some simple tampering
            if (!id.Equals(model.Id))
            {
                return new RepositoryActionResultModel<TModel>
                (
                    ResultType.FailedVerification,
                    PreviousVersion: model
                );
            }
            
            // Validate the incoming model against the registered validator
            ValidationResultModel validationResult = await _validator.ValidateUpdate(model);

            // Return a new result model if validation has failed
            if (!validationResult.IsValid)
            {
                return new RepositoryActionResultModel<TModel>
                (
                    ResultType.FailedValidation,
                    PreviousVersion: model,
                    ValidationResult: validationResult
                );
            }
            
            // Now we load the current entity version
            TEntity? currentEntity = await _context.GetEntity<TEntity>(id);
            
            // Check to see if the entity was found within the context
            if (currentEntity == null)
            {
                // If not, return a not found message
                return new RepositoryActionResultModel<TModel>
                (
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
            currentEntity.AuditInformation.UpdatedByUserId = _userToken.UserId;
            
            // Update in the context
            await _context.UpdateEntity(currentEntity);
            
            // Create a result containing old and new version, and return
            return new RepositoryActionResultModel<TModel>
            (
                ResultType.Success,
                PreviousVersion: currentModel,
                CurrentVersion: _mapper.Create(currentEntity)
            );
        }

        /// <summary>
        /// Marks an existing entry as deleted
        /// </summary>
        /// <param name="id">The id to mark as deleted</param>
        /// <returns>A response model containing any validation results with previous version of the model if successfully deleted</returns>
        public async Task<RepositoryActionResultModel<TModel>> Delete(string id)
        {
            // Could add some validation to see if the user is allowed to delete this type of entity
            //  as part of a rule based system...?
            
            // Now we load the current entity version
            TEntity? currentEntity = await _context.GetEntity<TEntity>(id);
            
            // Check to see if the entity was found within the context
            if (currentEntity == null)
            {
                // If not, return a not found message
                return new RepositoryActionResultModel<TModel>
                (
                    ResultType.NotFound
                );
            }
                        
            // Create a representation as it is today
            TModel currentModel = _mapper.Create(currentEntity);
            
            // Assign the defaults for updated flags
            currentEntity.AuditInformation ??= new BasicAuditInformation();
            currentEntity.AuditInformation.DateDeleted = DateTime.UtcNow;
            currentEntity.AuditInformation.DeletedByUserId = _userToken.UserId;
            currentEntity.EntityStatus = EntityStatus.Deleted;

            // Update in the context
            await _context.UpdateEntity(currentEntity);
            
            // Create a result containing old and new version, and return
            return new RepositoryActionResultModel<TModel>
            (
                ResultType.Success,
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
            if (currentEntity == null)
            {
                // If not, return a not found message
                return new RepositoryActionResultModel<TModel>
                (
                    ResultType.NotFound
                );
            }
                        
            // Create a result containing old and new version, and return
            return new RepositoryActionResultModel<TModel>
            (
                ResultType.Success,
                _mapper.Create(currentEntity)
            );
        }
    }
}