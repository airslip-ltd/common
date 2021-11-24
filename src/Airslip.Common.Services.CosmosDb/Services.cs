using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.CosmosDb.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.CosmosDb
{
    public static class Services
    {
        public static IServiceCollection AddAirslipCosmosDb<TContext>(this IServiceCollection services, 
            IConfiguration config, Func<Database, Task> initialiseCollections)
        where TContext : AirslipCosmosDbBase
        {
            services
                .Configure<CosmosDbSettings>(config.GetSection(nameof(CosmosDbSettings)))
                .AddSingleton(Helpers
                    .InitializeCosmosClientInstanceAsync(config, initialiseCollections)
                    .GetAwaiter()
                    .GetResult())
                .AddSingleton<TContext>()
                .AddSingleton<IContext>(provider => provider.GetService<TContext>()!);

            return services;
        }
    }
}