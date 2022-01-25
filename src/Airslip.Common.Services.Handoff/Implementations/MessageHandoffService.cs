using Airslip.Common.Services.Handoff.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Handoff.Implementations;

public class MessageHandoffService : IMessageHandoffService
{
    internal static readonly List<MessageHandoff> Handlers = new();
    private readonly IServiceProvider _provider;
    private readonly ILogger _logger;

    public MessageHandoffService(IServiceProvider provider, ILogger logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task ProcessMessage(string triggerName, string message)
    {
        _logger.Information("Triggered {TriggerName}", triggerName);

        if (!Handlers.Any(o => o.QueueName.Equals(triggerName)))
        {
            _logger.Fatal("Error in MessageHandoffService, handler not found for {TriggerName}", 
                triggerName);
            return;
        }
    
        try {
            MessageHandoff handler = Handlers
                .First(o => o.QueueName.Equals(triggerName));

            object? worker = _provider
                .GetService(handler.HandlerType);

            if (worker is not IMessageHandoffWorker messageHandoffWorker)
            {
                _logger.Fatal("Worker not found for {TriggerName}", 
                    triggerName);
                return;
            }
            
            await messageHandoffWorker.Execute(message, handler.DataSource);
        }
        catch (Exception ee)
        {
            _logger.Fatal(ee, "Uncaught error in {TriggerName}", triggerName);                
        }
        
        _logger.Information("Completed {TriggerName}", triggerName);
    }
}