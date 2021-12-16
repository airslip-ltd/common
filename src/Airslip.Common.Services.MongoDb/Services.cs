using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{
    public static class Services
    {
        public static IServiceCollection AddAirslipMongoDb<TContext>(this IServiceCollection services, 
            IConfiguration config, Func<IMongoDatabase, Task> initialiseDatabase)
        where TContext : AirslipMongoDbBase
        {
            services
                .Configure<MongoDbSettings>(config.GetSection(nameof(MongoDbSettings)))
                .AddSingleton(Helpers
                    .InitializeMongoClientInstanceAsync(config, initialiseDatabase)
                    .GetAwaiter()
                    .GetResult())
                .AddScoped<TContext>()
                .AddScoped<IContext>(provider => provider.GetService<TContext>()!);

            return services;
        }
    }
}
