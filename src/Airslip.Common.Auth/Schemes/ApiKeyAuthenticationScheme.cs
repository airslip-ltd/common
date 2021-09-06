using Microsoft.AspNetCore.Authentication;

namespace Airslip.Common.Auth.Schemes
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public static string ApiKeyScheme = "ApiKeyAuthScheme";
        public static string ApiKeyHeaderField = "x-api-key";
    }
}