using Airslip.SmartReceipts.Services.CosmosSql.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.CosmosDb
{
    internal static class Helpers
    {
        internal static async Task<CosmosClient> InitializeCosmosClientInstanceAsync(IConfiguration configuration,
            Func<Database, Task> initialiseCollections)
        {
            CosmosDbSettings settings = new();
            configuration.GetSection(nameof(CosmosDbSettings)).Bind(settings);

            CosmosClientOptions options = new()
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };
            
            CosmosClient client = new(settings.EndpointUri, settings.PrimaryKey, options);
            
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(settings.DatabaseName);
            
            await initialiseCollections(database.Database);

            return client;
        }
    }
}