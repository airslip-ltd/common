using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.EventHub.Implementations;
using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Services.EventHub
{
    public static class Services
    {
        public static IServiceCollection ConfigureServices(IServiceCollection serviceCollection,  
            IConfiguration configuration)
        {
            return serviceCollection
                .Configure<EventHubSettings>(configuration.GetSection(nameof(EventHubSettings)))
                .AddSingleton(typeof(IModelDeliveryService<>), typeof(EventHubModelDeliveryService<>));
        }
    }
}