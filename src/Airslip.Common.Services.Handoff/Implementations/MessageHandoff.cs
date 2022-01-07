using System;

namespace Airslip.Common.Services.Handoff.Implementations;

public class MessageHandoff
{
    public Type HandlerType { get; set; }

    public string QueueName { get; set; }
}