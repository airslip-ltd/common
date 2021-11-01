using Airslip.Common.Matching.Interfaces;

namespace Airslip.Matching.Yapily.Core.Interfaces
{
    public interface IEventHubFactory
    {
        IEventDeliveryService<TType> CreateInstance<TType>(string eventHubName);
    }
}