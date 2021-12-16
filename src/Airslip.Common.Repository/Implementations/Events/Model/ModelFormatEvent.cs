using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations.Events.Model
{
    public class ModelFormatEvent<TModel> : IModelPostProcessEvent<TModel> 
        where TModel : class, IModel
    {
        private readonly IEnumerable<IEntitySearchFormatter<TModel>> _searchFormatters;

        public ModelFormatEvent(IEnumerable<IEntitySearchFormatter<TModel>> searchFormatters)
        {
            _searchFormatters = searchFormatters;
        }

        public IEnumerable<LifecycleStage> AppliesTo { get; } = new[]
            {LifecycleStage.Get};

        public async Task<TModel> Execute(TModel model, LifecycleStage lifecycleStage)
        {
            foreach (IEntitySearchFormatter<TModel> searchFormatter in _searchFormatters)
            {
                model = await searchFormatter.FormatModel(model);
            }

            return model;
        }
    }
}