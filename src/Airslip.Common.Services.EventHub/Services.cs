using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.EventHub.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Services.EventHub
{
    public static class Services
    {
        public static IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton(typeof(IModelDeliveryService<>), typeof(EventHubModelDeliveryService<>));
        }
    }
}