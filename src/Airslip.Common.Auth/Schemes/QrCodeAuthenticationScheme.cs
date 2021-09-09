using Microsoft.AspNetCore.Authentication;

namespace Airslip.Common.Auth.Schemes
{
    public class QrCodeAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string QrCodeAuthScheme = "QrCodeAuthScheme";
    }
}