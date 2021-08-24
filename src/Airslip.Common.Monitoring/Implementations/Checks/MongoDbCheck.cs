using Airslip.Common.Monitoring.Interfaces;
using Airslip.Common.Monitoring.Models;
using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Monitoring.Implementations.Checks
{
    public class MongoDbCheck : IHealthCheck
    {
        private readonly ILogger _logger;
        private readonly MongoDbSettings _settings;

        public MongoDbCheck(IOptions<MongoDbSettings> settings, ILogger logger)
        {
            _logger = logger;
            _settings = settings.Value;
        }
        
        public async Task<HealthCheckResults> Execute()
        {
            HealthCheckResult checkResult = new(nameof(MongoDbCheck), _settings.DatabaseName, true, null);

            try
            {
                var client = new MongoClient(_settings.ConnectionString);
                var database = client.GetDatabase(_settings.DatabaseName);
                var collectionNames = await database.ListCollectionNamesAsync();

                while (await collectionNames.MoveNextAsync())
                {
                    _logger.Information("Found collections {CollectionNames}",string.Join(", ", collectionNames.Current));
                }
            }
            catch (Exception ee)
            {
                checkResult = new HealthCheckResult(nameof(MongoDbCheck), _settings.DatabaseName, false, ee);
            }

            return new HealthCheckResults(new []{ checkResult });
        }
    }
}