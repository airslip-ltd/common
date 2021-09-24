using Airslip.Common.Auth.Functions.Configuration;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class ApiRequestAuthService : IApiRequestAuthService
    {
        private readonly IApiKeyRequestDataHandler _requestDataHandler;
        private readonly ITokenDecodeService<ApiKeyToken> _decodeService;
        private readonly ApiAccessSettings _settings;

        public ApiRequestAuthService(IApiKeyRequestDataHandler requestDataHandler,
            ITokenDecodeService<ApiKeyToken> decodeService,
            IOptions<ApiAccessSettings> options)
        {
            _requestDataHandler = requestDataHandler;
            _decodeService = decodeService;
            _settings = options.Value;
        }

        public async Task<KeyAuthenticationResult> Handle(HttpRequestData requestData)
        {
            var authenticationResult = await _requestDataHandler.Handle(requestData);
            
            if (authenticationResult.AuthResult != AuthResult.Success)
            {
                return authenticationResult;
            }

            // Get the token
            var token = _decodeService.GetCurrentToken();
            
            // Validate against what is allowed in
            if (_settings.AllowedTypes.Count > 0 && !_settings
                .AllowedTypes.Contains(token.AirslipUserType))
            {
                return KeyAuthenticationResult.Fail("Invalid User Type supplied");
            }
            
            if (_settings.AllowedEntities.Count > 0 && !_settings
                .AllowedEntities.Contains(token.EntityId))
            {
                return KeyAuthenticationResult.Fail("Invalid Entity supplied");
            }

            return authenticationResult;
        }
    }
}