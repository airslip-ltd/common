using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.EventHub.Attributes;
using Airslip.Common.Services.EventHub.Extensions;
using Airslip.Common.Services.EventHub.Interfaces;
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
        private readonly IEventDeliveryService<TType>? _deliveryService;
        private readonly bool _supportsDelivery = true;
        
        public EventHubModelDeliveryService(IEventHubFactory eventHubFactory, ILogger logger)
        {
            _logger = logger;
            EventHubModelAttribute? attr = EventHubExtensions.GetAttributeByType<EventHubModelAttribute, TType>();

            if (attr == null)
            {
                _supportsDelivery = false;
                _logger.Debug("Model delivery to event hub not supported for this type");
            }
            else
            {
                _deliveryService = eventHubFactory.CreateInstance<TType>(attr.EventHubName);
            }
        }
        
        public async Task Deliver(TType model)
        {
            if (!_supportsDelivery) return;

            try
            {
                if (_deliveryService != null) 
                    await _deliveryService.DeliverEvents(model);
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "Error sending message");
            }
        }
    }
}