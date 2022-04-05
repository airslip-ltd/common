using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Services.EventHub.Interfaces
{
    public interface IEventDeliveryService<TType>
    {
        Task DeliverEvents(ICollection<TType> events);
        Task DeliverEvents(TType thisEvent);
    }
}