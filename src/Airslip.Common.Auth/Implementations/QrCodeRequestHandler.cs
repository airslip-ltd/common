using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Implementations
{
    public class QrCodeRequestHandler : IQrCodeRequestHandler
    {
        private readonly ITokenValidator<QrCodeToken, GenerateQrCodeToken> _tokenValidator;

        public QrCodeRequestHandler(ITokenValidator<QrCodeToken, GenerateQrCodeToken> tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }
        
        public async Task<AuthenticateResult> Handle(HttpRequest request)
        {
            // Get the raw querystring
            string qs = request.QueryString.ToString()[1..];

            if (qs.IsNullOrWhitespace())
            {
                return AuthenticateResult.Fail("QR Code invalid");
            }
            
            try
            {
                ClaimsPrincipal? apiKeyPrincipal = await _tokenValidator.GetClaimsPrincipalFromToken(qs, 
                    QrCodeAuthenticationSchemeOptions.QrCodeAuthScheme, 
                    QrCodeAuthenticationSchemeOptions.ThisEnvironment);

                return apiKeyPrincipal == null ? 
                    AuthenticateResult.Fail("QR Code invalid") : 
                    AuthenticateResult.Success(new AuthenticationTicket(apiKeyPrincipal, 
                        QrCodeAuthenticationSchemeOptions.QrCodeAuthScheme));
            }
            catch (ArgumentException)
            {
                return AuthenticateResult.Fail("QR Code invalid");
            }
            catch (EnvironmentUnsupportedException)
            {
                return AuthenticateResult.Fail("Environment Unsupported");
            }
        }
    }
}