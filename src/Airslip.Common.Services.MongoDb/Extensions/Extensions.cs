using Airslip.Common.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Airslip.Common.Services.MongoDb.Extensions
{
    public static class Extensions
    {
        public static IMongoCollection<TType> CollectionByType<TType>(this IMongoDatabase mongoDatabase)
        {
            return mongoDatabase.GetCollection<TType>(AirslipMongoDbBase.DeriveCollectionName<TType>());
        }

        public static bool CheckCollection(this IMongoDatabase mongoDatabase, string collectionName)
        {
            BsonDocument filter = new("name", collectionName);
            IAsyncCursor<BsonDocument> collectionCursor =
                mongoDatabase.ListCollections(new ListCollectionsOptions { Filter = filter });
            return collectionCursor.Any();
        }
        
        public static void CreateCollectionForEntity<TType>(this IMongoDatabase mongoDatabase) 
            where TType : IEntityWithId
        {
            // Map classes
            AirslipMongoDbBase.MapEntityWithId<TType>();

            string collectionName = AirslipMongoDbBase.DeriveCollectionName<TType>();

            if (!mongoDatabase.CheckCollection(collectionName))
                mongoDatabase.CreateCollection(collectionName);
        }
    }
}