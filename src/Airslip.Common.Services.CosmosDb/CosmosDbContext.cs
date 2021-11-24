using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Services.CosmosDb.Configuration;
using Airslip.Common.Services.CosmosDb.Extensions;
using Airslip.Common.Utilities.Extensions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pluralize.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airslip.Common.Services.CosmosDb
{
    public abstract class AirslipCosmosDbBase : IContext
    {
        protected readonly Database Database;
        private static readonly Pluralizer _pluralizer = new();
        
        protected AirslipCosmosDbBase(CosmosClient cosmosClient, IOptions<CosmosDbSettings> options)
        {
            Database = cosmosClient.GetDatabase(options.Value.DatabaseName);
        }

        public async Task<TEntity> AddEntity<TEntity>(TEntity newEntity) where TEntity : class, IEntityWithId
        {
            Container container = Database.GetContainerForEntity<TEntity>();
            ItemResponse<TEntity> result = await container.CreateItemAsync(newEntity, new PartitionKey(newEntity.Id));
            return result.Resource;
        }

        public async Task<TEntity?> GetEntity<TEntity>(string id) where TEntity : class, IEntityWithId
        {
            TEntity? result = null;
            Container container = Database.GetContainerForEntity<TEntity>();
            try
            {
                ItemResponse<TEntity> response = await container.ReadItemAsync<TEntity>(id, new PartitionKey(id));
                result = response.Resource;
            }
            catch(CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Do nothing as this is handled by the repository
            }
            
            return result;
        }

        public async Task<TEntity> UpdateEntity<TEntity>(TEntity updatedEntity) where TEntity : class, IEntityWithId
        {
            Container container = Database.GetContainerForEntity<TEntity>();
            await container.UpsertItemAsync(updatedEntity, new PartitionKey(updatedEntity.Id));
            return updatedEntity;
        }

        public async Task<List<TEntity>> GetEntities<TEntity>(List<SearchFilterModel> searchFilters) where TEntity : class, IEntityWithId
        {
            Container container = Database.GetContainerForEntity<TEntity>();
            StringBuilder sb = new();
            sb.Append($"SELECT * FROM {GetContainerId<TEntity>()} f WHERE 1=1");

            foreach (SearchFilterModel searchFilterModel in searchFilters)
            {
                sb.Append($" AND f.{searchFilterModel.FieldName.ToCamelCase()} = @{searchFilterModel.FieldName}");
            }

            QueryDefinition query = new(sb.ToString());

            foreach (SearchFilterModel searchFilterModel in searchFilters)
            {
                query = query.WithParameter($"@{searchFilterModel.FieldName}", searchFilterModel.FieldValue);
            }    
                
            using FeedIterator<TEntity> feedIterator = container.GetItemQueryIterator<TEntity>(
                query);
            
            return await FeedIteratorToResults(feedIterator);
        }

        public IQueryable<TEntity> QueryableOf<TEntity>() where TEntity : class
        {
            return Database.GetContainerForEntity<TEntity>()
                .GetItemLinqQueryable<TEntity>();
        }

        public async Task<TEntity> UpsertEntity<TEntity>(TEntity newEntity) where TEntity : class, IEntityWithId
        {
            Container container = Database.GetContainerForEntity<TEntity>();
            await container.UpsertItemAsync(newEntity, new PartitionKey(newEntity.Id));
            return newEntity;
        }

        public async Task<TEntity> Update<TEntity>(string id, string field, string value) where TEntity : class, IEntityWithId
        {
            Container container = Database.GetContainerForEntity<TEntity>();

            await container.PatchItemAsync<TEntity>(id, new PartitionKey(id), new[]
            {
                PatchOperation.Replace($"/{field}", value)
            });

            return (await GetEntity<TEntity>(id))!;
        }

        protected async Task<List<TEntity>> FeedIteratorToResults<TEntity>(FeedIterator<TEntity> feedIterator)
        {
            List<TEntity> results = new();
            
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<TEntity> response = await feedIterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
        
        public static string GetContainerId<TEntity>()
        {
            return _pluralizer.Pluralize(typeof(TEntity).Name);
        }
    }
}