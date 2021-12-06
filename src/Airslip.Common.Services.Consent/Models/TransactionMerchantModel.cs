namespace Airslip.SmartReceipts.Api.Core.Models
{
    public record TransactionMerchantModel
    {
        public TransactionMerchantModel()
        {
            
        }
        
        public TransactionMerchantModel(string entityId)
        {
            EntityId = entityId;
        }

        public string? EntityId { get; set; }
    }
}