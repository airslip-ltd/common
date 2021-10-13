using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{
        public abstract class AirslipMongoDbBase
    {
        protected readonly IMongoDatabase Database;

        protected AirslipMongoDbBase(IOptions<MongoDbSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            Database = mongoClient.GetDatabase(options.Value.DatabaseName);

            ConventionRegistry.Register(
                name: "CustomConventionPack", 
                conventions: new ConventionPack
                {
                    new CamelCaseElementNameConvention()
                },
                filter: t => true);
            
            // Enum as string
            BsonSerializer.RegisterSerializer(new EnumSerializer<AirslipUserType>(BsonType.String));
        }
        
        protected static void MapEntityWithId<TType>() where TType : IEntityWithId
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TType)))
            {
                BsonClassMap.RegisterClassMap<TType>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                });
            }
        }
        
        public IMongoCollection<TType> CollectionByType<TType>()
        {
            return Database.GetCollection<TType>(DeriveCollectionName<TType>());
        }
        
        public async Task<TEntity> AddEntity<TEntity>(TEntity newEntity)
            where TEntity : class, IEntityWithId
        {
            // Find appropriate collection
            IMongoCollection<TEntity> collection = CollectionByType<TEntity>();

            // Add entity to collection
            await collection.InsertOneAsync(newEntity);

            // Return the added entity - likely to be the same object
            return newEntity;
        }

        public async Task<TEntity?> GetEntity<TEntity>(string id)
            where TEntity : class, IEntityWithId
        {
            // Find appropriate collection
            IMongoCollection<TEntity> collection = CollectionByType<TEntity>();

            return await collection.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<TEntity> UpdateEntity<TEntity>(TEntity updatedEntity) where TEntity : class, IEntityWithId
        {
            IMongoCollection<TEntity> collection = CollectionByType<TEntity>();

            await collection.ReplaceOneAsync(user => user.Id == updatedEntity.Id, updatedEntity);

            return updatedEntity;
        }

        public Task<List<TEntity>> GetEntities<TEntity>(List<SearchFilterModel> searchFilters)
            where TEntity : class, IEntityWithId
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<TEntity> QueryableOf<TEntity>()
            where TEntity : class
        {
            IMongoCollection<TEntity> collection = CollectionByType<TEntity>();

            return collection.AsQueryable();
        }
        
        public async Task<TEntity> UpsertEntity<TEntity>(TEntity newEntity) where TEntity : class, IEntityWithId
        {
            
            // Find appropriate collection
            IMongoCollection<TEntity> collection = CollectionByType<TEntity>();

            // Add entity to collection
            await collection.ReplaceOneAsync(
                entity => entity.Id == newEntity.Id, newEntity, new ReplaceOptions
                {
                    IsUpsert = true
                });

            // Return the added entity - likely to be the same object
            return newEntity;
        }

        protected void CreateCollection<TType>() where TType : IEntityWithId
        {
            // Map classes
            MapEntityWithId<TType>();
            
            string collectionName = DeriveCollectionName<TType>();
            
            if (!_checkCollection(collectionName))
                Database.CreateCollection(collectionName);
        }

        private bool _checkCollection(string collectionName)
        {
            BsonDocument filter = new("name", collectionName);
            IAsyncCursor<BsonDocument> collectionCursor =
                Database.ListCollections(new ListCollectionsOptions {Filter = filter});
            return collectionCursor.Any();
        }

        private static string DeriveCollectionName<TType>()
        {
            return $"{typeof(TType).Name}s".ToCamelCase();
        }
    }
}