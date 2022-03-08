using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations.Events.Model.PreProcess;

public class ModelTimeStampEvent<TModel> : IModelPreProcessEvent<TModel> 
    where TModel : class, IModel
{
    public IEnumerable<LifecycleStage> AppliesTo { get; } = new[]
        {LifecycleStage.Create, LifecycleStage.Update, LifecycleStage.Delete};

    public Task<TModel> Execute(TModel model, LifecycleStage lifecycleStage)
    {
        if (!lifecycleStage.CheckApplies(AppliesTo)) return Task.FromResult(model);
        if (model is not IModelWithTimeStamp modelWithTimeStamp) return Task.FromResult(model);

        modelWithTimeStamp.TimeStamp = DateTime.UtcNow.ToUnixTimeMilliseconds();
        
        return Task.FromResult(model);
    }
}