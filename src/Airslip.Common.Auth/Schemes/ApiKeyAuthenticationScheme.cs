using Microsoft.AspNetCore.Authentication;

namespace Airslip.Common.Auth.Schemes
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string ApiKeyScheme = "ApiKeyAuthScheme";
        public const string ApiKeyHeaderField = "x-api-key";
        
        public static string ThisEnvironment { get; set; } = "Development";

        public string Environment
        {
            set
            {
                ThisEnvironment = value;
            }
        }
    }
}