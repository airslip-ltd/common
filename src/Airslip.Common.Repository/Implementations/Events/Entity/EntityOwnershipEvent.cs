using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Exception;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Enums;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Implementations.Events.Entity
{
    public class EntityOwnershipEvent<TEntity> : IEntityPreProcessEvent<TEntity> 
        where TEntity : class, IEntity
    {
        private readonly IRepositoryUserService _userService;

        public EntityOwnershipEvent(IRepositoryUserService userService)
        {
            _userService = userService;
        }
        public IEnumerable<LifecycleStage> AppliesTo => new[]
            {LifecycleStage.Create};
        public TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
        {
            if (entity is not IEntityWithOwnership entityWithOwnership) 
                return entity;
            
            if (_userService.AirslipUserType is null)
            {
                throw new RepositoryLifecycleException(ErrorCodes.OwnershipCannotBeDerived, false);
            }
                
            entityWithOwnership.AirslipUserType =
                entityWithOwnership.AirslipUserType == AirslipUserType.Unknown ?
                    _userService.AirslipUserType.Value : entityWithOwnership.AirslipUserType;

            // Bind the UserId and EntityId where available
            switch (_userService.AirslipUserType.Value)
            { 
                case AirslipUserType.Standard:
                    entityWithOwnership.UserId ??= _userService.UserId;
                    break;
                    
                default:
                    entityWithOwnership.UserId ??= _userService.UserId;
                    entityWithOwnership.EntityId ??= _userService.EntityId;
                    break;
            }

            return entity;
        }
    }
}