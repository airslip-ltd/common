namespace Airslip.Common.Types.Configuration
{
    public class PublicApiSettings
    {
        public PublicApiSetting Base { get; set; } = new PublicApiSetting();
        public PublicApiSetting? MerchantTransactions { get; set; }
        public PublicApiSetting? MerchantDatabase { get; set; }
        public PublicApiSetting? Identity { get; set; }
        public PublicApiSetting? BankTransactions { get; set; }
    }

    public class PublicApiSetting 
    {
        public string BaseUri { get; set; } = string.Empty;
        public string UriSuffix { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }
}