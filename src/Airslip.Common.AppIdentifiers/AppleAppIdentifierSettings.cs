namespace Airslip.Common.AppIdentifiers
{
    public class AppleAppIdentifierSettings
    {
        public AppleAppIdentifierSetting BankTransactions { get; set; } = new();
        public AppleAppIdentifierSetting Identity { get; set; } = new();
    }
    
    public class AppleAppIdentifierSetting
    {
        public string BaseUri { get; set; } = string.Empty;
        public string UriSuffix { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string AppID { get; set; } = string.Empty;
    }
}