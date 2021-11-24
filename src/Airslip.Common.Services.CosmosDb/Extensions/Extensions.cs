using Microsoft.Azure.Cosmos;
using Pluralize.NET.Core;
using System.Threading.Tasks;

namespace Airslip.Common.Services.CosmosDb.Extensions
{
    public static class Extensions
    {
        public static async Task CreateCollection<TEntity>(this Database database, string partitionKeyPath = "id")
        {
            string containerId = CosmosDbContext.GetContainerId<TEntity>();
            
            await database.CreateContainerIfNotExistsAsync(containerId, $"/{partitionKeyPath}");
        }
        
        public static Container GetContainerForEntity<TEntity>(this Database database)
        {
            string containerId = CosmosDbContext.GetContainerId<TEntity>();
            return database.GetContainer(containerId);
        }
    }
}