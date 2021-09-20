namespace Airslip.Common.Auth.Data
{
    public static class AirslipSchemeOptions
    {
        public const string ApiKeyScheme = "ApiKeyAuthScheme";
        public const string ApiKeyHeaderField = "x-api-key";
        public const string QrCodeAuthScheme = "QrCodeAuthScheme";
        
        public static string ThisEnvironment { get; set; } = "Development";
    }
}