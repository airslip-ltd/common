using Microsoft.Extensions.DependencyInjection;
using System;

namespace Airslip.Common.Services.EventHub.Extensions
{
    public static class EventHubExtensions
    {
        public static IServiceCollection AddEventHubModelDelivery(this IServiceCollection services)
        {
            return Services.ConfigureServices(services);
        }
        
        public static TAtt? GetAttributeByType<TAtt, TType>() where TAtt : Attribute
        {
            // Using reflection.  
            Attribute[] attrs = Attribute.GetCustomAttributes(typeof(TType));  // Reflection.  
  
            // Displaying output.  
            foreach (Attribute attr in attrs)  
            {  
                if (attr is TAtt att)
                {
                    return att;
                }  
            }

            return null;
        }
    }
}