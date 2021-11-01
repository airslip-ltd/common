using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IEventDeliveryService<TType>
    {
        Task DeliverEvents(List<TType> events);
        Task DeliverEvents(TType thisEvent);
    }
}