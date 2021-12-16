using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations
{
    public class RepositoryLifecycle<TEntity, TModel> : IRepositoryLifecycle<TEntity, TModel>
        where TEntity : class, IEntity 
        where TModel : class, IModel
    {
        private readonly IEnumerable<IEntityPreProcessEvent<TEntity>> _entityPreProcessEvents;
        private readonly IEnumerable<IEntityPostProcessEvent<TEntity>> _entityPostProcessEvents;
        private readonly IEnumerable<IModelPostProcessEvent<TModel>> _modelPostProcessEvents;

        public RepositoryLifecycle(IEnumerable<IEntityPreProcessEvent<TEntity>> entityPreProcessEvents,
            IEnumerable<IEntityPostProcessEvent<TEntity>> entityPostProcessEvents,
            IEnumerable<IModelPostProcessEvent<TModel>> modelPostProcessEvents)
        {
            _entityPreProcessEvents = entityPreProcessEvents;
            _entityPostProcessEvents = entityPostProcessEvents;
            _modelPostProcessEvents = modelPostProcessEvents;
        }
        
        public TEntity PreProcess(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
        {
            return _entityPreProcessEvents.Where(o => o.AppliesTo.Contains(lifecycleStage))
                .Aggregate(entity, (current, preProcessEvent) => preProcessEvent.Execute(current, lifecycleStage, userId));
        }

        public TEntity PostProcess(TEntity entity, LifecycleStage lifecycleStage, string? userId = null)
        {
            return _entityPostProcessEvents.Where(o => o.AppliesTo.Contains(lifecycleStage))
                .Aggregate(entity, (current, postProcessEvent) => postProcessEvent.Execute(current, lifecycleStage, userId));
        }

        public async Task<TModel> PostProcessModel(TModel model, LifecycleStage lifecycleStage)
        {
            foreach (IModelPostProcessEvent<TModel> postProcessEvent in _modelPostProcessEvents
                         .Where(o => o.AppliesTo.Contains(lifecycleStage)))
            {
                await postProcessEvent.Execute(model, lifecycleStage);
            }

            return model;
        }
    }
}