using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Testing;
using Airslip.Common.Types;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Services.MongoDb.Tests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private MongoDbSettings _mongoDbSettings = null!;
        public IContext Context = null!;

        public async Task InitializeAsync()
        {
            IConfiguration config = TestFactory.InitialiseConfiguration(Directory.GetCurrentDirectory());
            _mongoDbSettings = UtilityExtensions.GetConfigurationSection<MongoDbSettings>(config);
            _mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = _mongoDbSettings.ConnectionString,
                DatabaseName = $"tests_{CommonFunctions.GetId()}"
            };
            Context = new MyBaseTestClass(Options.Create(_mongoDbSettings));
            
            string[] names = {"Some Name 1", "Some Name 2", "Some Name 3", "Some Name 4"};
            
            // Prepare some test data
            foreach (string name in names)
            {
                await Context.AddEntity(new MyEntity
                {
                    Id = CommonFunctions.GetId(),
                    Name = name
                });
            }
        }

        public async Task DisposeAsync()
        {
            MongoClient client = new(_mongoDbSettings.ConnectionString);
            await client.DropDatabaseAsync(_mongoDbSettings.DatabaseName);
        }
    }
    
    public class MyBaseTestClass : AirslipMongoDbBase
    {
        public MyBaseTestClass(IOptions<MongoDbSettings> options) : base(options)
        {
            CreateCollection<MyEntity>();
        }
    }

    public class MyEntity : IEntityWithId
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}