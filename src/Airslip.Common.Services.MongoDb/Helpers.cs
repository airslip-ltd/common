using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Enums;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{
    public static class Helpers
    {
        public static async Task<MongoClient> InitializeMongoClientInstanceAsync(IConfiguration configuration,
            Func<IMongoDatabase, Task> initialiseDatabase)
        {
            MongoDbSettings settings = new();
            configuration.GetSection(nameof(MongoDbSettings)).Bind(settings);

            MongoClient mongoClient = new(settings.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(settings.DatabaseName);
            
            await initialiseDatabase(database);

            // General initialisation
            ConventionRegistry.Register(
                "CustomConventionPack",
                new ConventionPack
                {
                    new CamelCaseElementNameConvention()
                },
                t => true);

            // Enum as string
            BsonSerializer.RegisterSerializer(new EnumSerializer<AirslipUserType>(BsonType.String));

            return mongoClient;
        }
    }
}