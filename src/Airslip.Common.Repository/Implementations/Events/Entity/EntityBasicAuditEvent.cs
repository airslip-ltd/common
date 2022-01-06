using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Implementations.Events.Entity
{
    public class EntityBasicAuditEvent<TEntity> : IEntityPreProcessEvent<TEntity> 
        where TEntity : class, IEntity
    {
        private readonly IUserContext _userService;

        public EntityBasicAuditEvent(IUserContext userService)
        {
            _userService = userService;
        }
        public IEnumerable<LifecycleStage> AppliesTo => new[]
            {LifecycleStage.Create, LifecycleStage.Delete, LifecycleStage.Update};
        public TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
        {
            entity.AuditInformation ??= new BasicAuditInformation
            {
                Id = CommonFunctions.GetId()
            };

            switch (lifecycleStage)
            {
                case LifecycleStage.Create:
                    entity.AuditInformation.DateCreated = DateTime.UtcNow;
                    entity.AuditInformation.CreatedByUserId = userId ?? _userService.UserId;
                    break;
                case LifecycleStage.Update:
                    entity.AuditInformation.DateUpdated = DateTime.UtcNow;
                    entity.AuditInformation.UpdatedByUserId = userId ?? _userService.UserId;
                    break;
                case LifecycleStage.Delete:
                    entity.AuditInformation.DateDeleted = DateTime.UtcNow;
                    entity.AuditInformation.DeletedByUserId = userId ?? _userService.UserId;
                    break;
            }

            return entity;
        }
    }
}