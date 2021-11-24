using Airslip.Common.Repository.Interfaces;
using Airslip.SmartReceipts.Services.CosmosSql.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.CosmosDb
{
    public static class Services
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, 
            IConfiguration config, Func<Database, Task> initialiseCollections)
        {
            services
                .Configure<CosmosDbSettings>(config.GetSection(nameof(CosmosDbSettings)))
                .AddSingleton(CosmosDbContext
                    .InitializeCosmosClientInstanceAsync(config, initialiseCollections)
                    .GetAwaiter()
                    .GetResult())
                .AddSingleton<CosmosDbContext>()
                .AddSingleton<IContext>(provider => provider.GetService<CosmosDbContext>()!);

            return services;
        }
    }
}