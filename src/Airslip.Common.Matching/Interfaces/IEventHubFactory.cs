namespace Airslip.Common.Matching.Interfaces
{
    public interface IEventHubFactory
    {
        IEventDeliveryService<TType> CreateInstance<TType>(string eventHubName);
    }
}