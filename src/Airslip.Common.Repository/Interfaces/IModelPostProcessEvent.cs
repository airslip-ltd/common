using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Interfaces
{
    public interface IModelPostProcessEvent<TModel>
        where TModel : class, IModel
    {
        IEnumerable<LifecycleStage> AppliesTo { get; }
        Task<TModel> Execute(TModel model, LifecycleStage lifecycleStage);
    }
}