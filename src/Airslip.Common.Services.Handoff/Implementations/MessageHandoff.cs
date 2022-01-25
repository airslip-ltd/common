using Airslip.Common.Types.Enums;
using System;

namespace Airslip.Common.Services.Handoff.Implementations;

public class MessageHandoff
{
    public MessageHandoff(Type handlerType, string queueName, DataSources dataSource)
    {
        HandlerType = handlerType;
        QueueName = queueName;
        DataSource = dataSource;
    }

    public Type HandlerType { get; init; }
    public string QueueName { get; init; }
    public DataSources DataSource { get; init; }
}