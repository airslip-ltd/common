using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Schemes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Handlers
{
    public class QrCodeAuthHandler : AuthenticationHandler<QrCodeAuthenticationSchemeOptions>
    {
        private readonly IQrCodeRequestHandler _qrCodeRequestHandler;

        public QrCodeAuthHandler(IOptionsMonitor<QrCodeAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, 
            IQrCodeRequestHandler qrCodeRequestHandler) 
            : base(options, logger, encoder, clock)
        {
            _qrCodeRequestHandler = qrCodeRequestHandler;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await _qrCodeRequestHandler.Handle(Request);
        }
    }
}