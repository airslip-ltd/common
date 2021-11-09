using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Airslip.Common.Services.MongoDb
{
    public abstract partial class AirslipMongoDbBase : IContext
    {
        protected readonly IMongoDatabase Database;

        protected AirslipMongoDbBase(IOptions<MongoDbSettings> options)
        {
            MongoClient mongoClient = new(options.Value.ConnectionString);
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
            BsonSerializer.RegisterSerializer(new EnumSerializer<EntityStatus>(BsonType.String));
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

        public IQueryable<TEntity> QueryableOf<TEntity>()
            where TEntity : class
        {
            IMongoCollection<TEntity> collection = CollectionByType<TEntity>();

            return collection.AsQueryable();
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
                Database.ListCollections(new ListCollectionsOptions { Filter = filter });
            return collectionCursor.Any();
        }

        private static string DeriveCollectionName<TType>()
        {
            return $"{typeof(TType).Name}s".ToCamelCase();
        }
        
        public void SetupIndex<TEntity>(IndexTypes indexType, string field) where TEntity : class, IEntityWithId 
        {
            CreateIndexOptions indexOptions = new();

            IndexKeysDefinitionBuilder<TEntity> indexBuilder = Builders<TEntity>.IndexKeys;

            IMongoCollection<TEntity> collection = CollectionByType<TEntity>();

            IndexKeysDefinition<TEntity>? indexKeysDefinition = indexType switch
            {
                IndexTypes.Ascending => indexBuilder.Ascending(field),
                IndexTypes.Descending => indexBuilder.Descending(field),
                _ => throw new ArgumentOutOfRangeException(nameof(indexType), indexType, null)
            };

            collection.Indexes.CreateOneAsync(
                new CreateIndexModel<TEntity>(
                    indexKeysDefinition,
                    indexOptions));
        }
    }
}