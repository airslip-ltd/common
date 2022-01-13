using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations.Events.Entity.PreValidate;

public class EntityOwnershipValidationEvent<TEntity, TModel> : IEntityPreValidateEvent<TEntity, TModel> 
    where TEntity : class, IEntity 
    where TModel : class, IModel
{
    private readonly IUserContext _userService;

    public EntityOwnershipValidationEvent(IUserContext userService)
    {
        _userService = userService;
    }
    public IEnumerable<LifecycleStage> AppliesTo => new[]
        {LifecycleStage.Update, LifecycleStage.Delete, LifecycleStage.Get};

    public Task<List<ValidationResultMessageModel>> Validate(RepositoryAction<TEntity, TModel> repositoryAction)
    {
        List<ValidationResultMessageModel> result = new(); 
        result.AddRange(repositoryAction.CheckApplies(AppliesTo));
        
        if (repositoryAction.Entity is not IEntityWithOwnership entityWithOwnership) 
            return Task.FromResult(result);
            
        if (!entityWithOwnership.CanView(_userService))
        {
            result.Add(new ValidationResultMessageModel(nameof(IEntityWithOwnership.EntityId), 
                ErrorMessages.OwnershipNotVerified));
        }

        return Task.FromResult(result);
    }
}