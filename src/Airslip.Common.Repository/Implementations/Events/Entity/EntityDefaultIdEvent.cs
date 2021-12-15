using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Utilities;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Implementations.Events.Entity
{
    public class EntityDefaultIdEvent<TEntity> : IEntityPreProcessEvent<TEntity> 
        where TEntity : class, IEntity
    {
        public IEnumerable<LifecycleStage> AppliesTo => new[]
            {LifecycleStage.Create};
        public TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
        {
            entity.Id = string.IsNullOrWhiteSpace(entity.Id) ? CommonFunctions.GetId() : entity.Id;


            return entity;
        }
    }
}