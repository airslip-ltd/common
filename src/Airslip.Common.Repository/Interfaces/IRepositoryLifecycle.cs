using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Interfaces
{
    public interface IRepositoryLifecycle<TEntity, TModel> 
        where TEntity : class, IEntity 
        where TModel : class, IModel
    {
        TEntity PreProcess(TEntity entity, LifecycleStage lifecycleStage, string? userId = null);
        TEntity PostProcess(TEntity entity, LifecycleStage lifecycleStage, string? userId = null);
        Task<TModel> PostProcess(TModel model, LifecycleStage lifecycleStage);
    }
    
    public interface IEntityPreProcessEvent<TEntity>
        where TEntity : class, IEntity 
    {
        IEnumerable<LifecycleStage> AppliesTo { get; }
        TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null);
    }
    
    public interface IEntityPostProcessEvent<TEntity>
        where TEntity : class, IEntity 
    {
        IEnumerable<LifecycleStage> AppliesTo { get; }
        TEntity Execute(TEntity entity, LifecycleStage lifecycleStage, string? userId = null);
    }
    
    public interface IModelPostProcessEvent<TModel>
        where TModel : class, IModel
    {
        IEnumerable<LifecycleStage> AppliesTo { get; }
        Task<TModel> Execute(TModel model, LifecycleStage lifecycleStage);
    }
    
    public enum LifecycleStage
    {
        Create,
        Update,
        Delete,
        Get
    }
}