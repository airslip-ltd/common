using Microsoft.Azure.Cosmos;

namespace Airslip.SmartReceipts.Services.CosmosSql.Configuration
{
    public class CosmosDbSettings
    {
        public string PrimaryKey { get; set; } = string.Empty;
        public string EndpointUri { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}