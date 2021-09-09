using Airslip.Common.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Repository
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped(typeof(IRepository<,>), typeof(Implementations.Repository<,>));
        }
    }
}