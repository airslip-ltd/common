namespace Airslip.Common.Types.Configuration
{
    public class PublicApiSettings
    {
        public PublicApiSetting Base { get; set; } = new();
        public PublicApiSetting? MerchantTransactions { get; set; }
        public PublicApiSetting? MerchantDatabase { get; set; }
        public PublicApiSetting? Identity { get; set; }
        public PublicApiSetting? BankTransactions { get; set; }
        public PublicApiSetting? Notifications { get; set; }
        public PublicApiSetting? QrCodeMatching { get; set; }
    }
}