using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.EventHub.Attributes;
using Airslip.Common.Services.EventHub.Extensions;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Utilities;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.EventHub.Implementations
{
    public class EventHubModelDeliveryService<TType> : IModelDeliveryService<TType> 
        where TType : class, IModel
    {
        private readonly ILogger _logger;
        private readonly EventHubProducerClient? _producerClient = null;
        private bool _supportsDelivery = true;
        
        public EventHubModelDeliveryService(IOptions<EventHubSettings> options, ILogger logger)
        {
            _logger = logger;
            EventHubModelAttribute? attr = EventHubExtensions.GetAttributeByType<EventHubModelAttribute, TType>();

            if (attr == null)
            {
                _supportsDelivery = false;
                _logger.Information("Model delivery to event hub not supported for this type");
            }
            else
            {
                EventHubSettings eventHubSettings = options.Value;
                _producerClient = new EventHubProducerClient(eventHubSettings.ConnectionString, 
                    attr.EventHubName);
            }
        }
        
        public async Task Deliver(TType model)
        {
            if (!_supportsDelivery) return;
            
            // Create a batch of events 
            using EventDataBatch eventBatch = await _producerClient!.CreateBatchAsync();

            // Convert envelope to a string
            string message = Json.Serialize(model);
            
            if (!eventBatch.TryAdd(new EventData(message)))
            {
                // if it is too large for the batch
                throw new ArgumentException("Event is too large for the batch and cannot be sent.");
            }

            try
            {
                // Use the producer client to send the batch of events to the event hub
                await _producerClient.SendAsync(eventBatch);
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "Error sending message");
            }
        }
    }
}