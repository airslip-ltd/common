using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Implementations.Events.Entity
{
    public class EntityStatusEvent<TEntity> : IEntityPreProcessEvent<TEntity> 
        where TEntity : class, IEntity
    {
        public IEnumerable<LifecycleStage> AppliesTo => new[]
            {LifecycleStage.Create, LifecycleStage.Delete, LifecycleStage.Update};
        public TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
        {
            switch (lifecycleStage)
            {
                case LifecycleStage.Create:
                case LifecycleStage.Update:
                    entity.EntityStatus = EntityStatus.Active;
                    break;
                case LifecycleStage.Delete:
                    entity.EntityStatus = EntityStatus.Deleted;
                    break;
            }

            return entity;
        }
    }
}