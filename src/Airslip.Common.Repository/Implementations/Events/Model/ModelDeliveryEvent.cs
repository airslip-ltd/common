using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations.Events.Model
{
    public class ModelDeliveryEvent<TModel> : IModelPostProcessEvent<TModel> 
        where TModel : class, IModel
    {
        private readonly IEnumerable<IModelDeliveryService<TModel>> _deliveryServices;

        public ModelDeliveryEvent(IEnumerable<IModelDeliveryService<TModel>> deliveryServices)
        {
            _deliveryServices = deliveryServices;
        }

        public IEnumerable<LifecycleStage> AppliesTo { get; } = new[]
            {LifecycleStage.Create, LifecycleStage.Delete, LifecycleStage.Update};

        public async Task<TModel> Execute(TModel model, LifecycleStage lifecycleStage)
        {
            foreach (IModelDeliveryService<TModel> modelDeliveryService in _deliveryServices)
            {
                await modelDeliveryService.Deliver(model);
            }

            return model;
        }
    }
}