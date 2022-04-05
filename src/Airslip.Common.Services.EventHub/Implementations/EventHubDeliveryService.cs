using Airslip.Common.Services.EventHub.Interfaces;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Utilities;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Services.EventHub.Implementations;

public class EventHubDeliveryService<TType> : IEventDeliveryService<TType>
{
    private readonly ILogger _logger;
    private readonly EventHubProducerClient _producerClient;   
        
    public EventHubDeliveryService(IOptions<EventHubSettings> options, string eventHubName, ILogger logger)
    {
        _logger = logger;
        EventHubSettings? eventHubSettings = options.Value;
        _producerClient = new EventHubProducerClient(eventHubSettings.ConnectionString, 
            eventHubName);
    }
        
    public async Task DeliverEvents(ICollection<TType> events)
    {
        // Create a batch of events
        try
        {
            await _executeChunked(
                data => _producerClient.SendAsync(data),
                model => new EventData(Json.Serialize(model)),
                events, 
                10);
        }
        catch (Exception eee)
        {
            _logger.Fatal(eee, "Error sending events");
        }
    }

    public async Task DeliverEvents(TType thisEvent)
    {
        await DeliverEvents(new[] { thisEvent });
    }

    private static async Task _executeChunked<TExecuteType, TListType>(
        Func<IEnumerable<TExecuteType>, Task> executeMe, 
        Func<TListType, TExecuteType> createMe,
        ICollection<TListType> myCollection, 
        int chunkSize) 
    {
        // Create a batch of events
        List<TExecuteType> batchList = new();
        int batchCount = 0;
            
        foreach (TListType model in myCollection)
        {
            // Convert envelope to a string
            batchList.Add(createMe(model));
            batchCount += 1;

            if (batchCount < chunkSize) continue;
                
            await executeMe(batchList);

            batchList.Clear();
            batchCount = 0;
        }
            
        if  (batchList.Any()) 
            await executeMe(batchList);
    }
}