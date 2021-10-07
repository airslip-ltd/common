using Airslip.Common.Auth.AspNetCore.Configuration;
using Airslip.Common.Auth.AspNetCore.Interfaces;
using Airslip.Common.Auth.AspNetCore.Schemes;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types;
using Airslip.Common.Types.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Text;

namespace Airslip.Common.Auth.AspNetCore.Implementations
{
    public class CookieService : ICookieService
    {
        private readonly ITokenGenerationService<GenerateUserToken> _tokenGenerationService;
        private readonly ILogger _logger;
        private readonly CookieSettings _cookieSettings;
        private readonly HttpContext _context;

        public CookieService(ITokenGenerationService<GenerateUserToken> tokenGenerationService,
            IHttpContextAccessor httpContextAccessor,
            ILogger logger,
            IOptions<CookieSettings> cookieSettings)
        {
            _tokenGenerationService = tokenGenerationService;
            _logger = logger;
            _cookieSettings = cookieSettings.Value;
            _context = httpContextAccessor.HttpContext ?? 
                       throw new ArgumentException("HttpContext cannot be null");
        }
        
        public void UpdateCookie(UserToken userToken)
        {
            NewToken newToken = _generateNewToken(userToken);
            
            // Add to the response as a new cookie
            string random = CommonFunctions.GetId();
            string passphrase = $"{_cookieSettings.Passphrase}{random}";
            string base64encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(random));
            CookieOptions options = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Strict,
                HttpOnly = true
            };
            
            _context.Response.Cookies.Append(CookieSchemeOptions.CookieEncryptField, base64encoded, options);
            _context.Response.Cookies.Append(CookieSchemeOptions.CookieTokenField, 
                StringCipher.Encrypt(newToken.TokenValue, passphrase), options);
        }

        public string GetCookieValue(HttpRequest request)
        {
            string cookieValue = request.Cookies[CookieSchemeOptions.CookieTokenField] ?? string.Empty;
            string base64encoded = request.Cookies[CookieSchemeOptions.CookieEncryptField] ?? string.Empty;
            string passphrase = Encoding.UTF8.GetString(Convert.FromBase64String(base64encoded));
            
            // Otherwise, decrypt!
            string wholePassphrase = $"{_cookieSettings.Passphrase}{passphrase}";
            
            // Decrypt token
            string decryptedToken;
            try
            {
                decryptedToken = StringCipher.Decrypt(cookieValue, wholePassphrase);
            }
            catch (Exception ee)
            {
                _logger.Error(ee, "Error decoding cookie");
                throw new ArgumentException("Cookie invalid");
            }

            return decryptedToken;
        }
        
        private NewToken _generateNewToken(UserToken userToken)
        {
            GenerateUserToken generate = new(userToken.EntityId, 
                userToken.AirslipUserType, userToken.UserId, userToken.YapilyUserId);
            
            return _tokenGenerationService.GenerateNewToken(generate);
        }
    }
}