using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Exception;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Implementations.Events.Entity
{
    public class EntityOwnershipValidationEvent<TEntity> : IEntityPreProcessEvent<TEntity> 
        where TEntity : class, IEntity
    {
        private readonly IUserContext _userService;

        public EntityOwnershipValidationEvent(IUserContext userService)
        {
            _userService = userService;
        }
        public IEnumerable<LifecycleStage> AppliesTo => new[]
            {LifecycleStage.Update, LifecycleStage.Delete};
        public TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
        {
            if (entity is not IEntityWithOwnership entityWithOwnership) 
                return entity;
            
            if (!entityWithOwnership.CanView(_userService))
            {
                throw new RepositoryLifecycleException(ErrorCodes.OwnershipCannotBeVerified, false);
            }

            return entity;
        }
    }
}