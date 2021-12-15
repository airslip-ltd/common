using Airslip.Common.Repository.Enums;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Interfaces
{
    public interface IRepositoryLifecycle<TEntity, TModel> 
        where TEntity : class, IEntity 
        where TModel : class, IModel
    {
        TEntity PreProcess(TEntity entity, LifecycleStage lifecycleStage, string? userId = null);
        TEntity PostProcess(TEntity entity, LifecycleStage lifecycleStage, string? userId = null);
        Task<TModel> PostProcessModel(TModel model, LifecycleStage lifecycleStage);
    }
}