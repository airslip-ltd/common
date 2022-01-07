using Airslip.Common.Services.Handoff.Implementations;
using Airslip.Common.Services.Handoff.Interfaces;

namespace Airslip.Common.Services.Handoff.Extensions;

public static class Extensions
{
    public static MessageHandoffOptions Register<THandoffProcessor>(this MessageHandoffOptions messageHandoff, string queueName)
        where THandoffProcessor : IMessageHandoffWorker
    {
        MessageHandoffService.Handlers.Add(new MessageHandoff
        {
            QueueName = queueName,
            HandlerType =  typeof(THandoffProcessor)
        });

        return messageHandoff;
    }
}

public class MessageHandoffOptions
{
}