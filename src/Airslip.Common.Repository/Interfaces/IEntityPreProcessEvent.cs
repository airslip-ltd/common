using Airslip.Common.Repository.Enums;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Interfaces
{
    public interface IEntityPreProcessEvent<TEntity>
        where TEntity : class, IEntity 
    {
        IEnumerable<LifecycleStage> AppliesTo { get; }
        TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null);
    }
}