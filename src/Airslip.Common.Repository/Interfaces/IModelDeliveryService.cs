using System.Threading.Tasks;

namespace Airslip.Common.Repository.Interfaces
{
    public interface IModelDeliveryService<in TModel>
        where TModel : class, IModel
    {
        Task Deliver(TModel model);
    }
}