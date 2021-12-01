using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace Airslip.Common.Auth.UnitTests.Helpers
{
    public static class HelperFunctions
    {
        
        public static Mock<IOptions<JwtSettings>> GenerateOptionsWithKey(string key)
        {
            JwtSettings settings = new()
            {
                Audience = "Some Audience",
                Issuer = "Some Issuer",
                Key = key,
                ExpiresTime = 3600,
                ValidateLifetime = false
            };
            Mock<IOptions<JwtSettings>> options = new();
            options.Setup(_ => _.Value).Returns(settings);

            return options;
        }

        public static IRemoteIpAddressService GenerateRemoteIpAddressService(string withForwarder = null,
            string withRemoteAddr = null)
        {
            Mock<IHttpContextAccessor> context = ContextHelpers.GenerateContext("", TokenType.ApiKey, 
                forwarder: withForwarder, remoteAddr:withRemoteAddr);
            RemoteIpAddressService service = new(context.Object);

            return service;
        }
        
        public static Mock<IRemoteIpAddressService> GenerateMockRemoteIpAddressService(string ipAddress)
        {
            Mock<IRemoteIpAddressService> remoteIpAddressService = new();
            remoteIpAddressService.Setup(_ => _.GetRequestIP()).Returns(ipAddress);

            return remoteIpAddressService;
        }

        public static IUserAgentService GenerateUserAgentService(string withUserAgent = Constants.UA_WINDOWS_10_EDGE)
        {
            Mock<IHttpContextAccessor> accessor = ContextHelpers.GenerateContext("", TokenType.BearerToken, 
                withUserAgent);
            UserAgentService userAgentService = new(accessor.Object);

            return userAgentService;
        }

        public static ITokenValidator<TTokenType> GenerateValidator<TTokenType>(TokenType tokenType) where TTokenType : IDecodeToken, new()
        {
            TokenValidator<TTokenType> apiKeyValidator = 
                new(CreateTokenDecodeService<TTokenType>("", tokenType));

            return apiKeyValidator;
        }
        
        // User specifics
        public static ITokenGenerationService<GenerateUserToken> CreateUserTokenGenerationService(string withIpAddress, 
            string withUserAgent = Constants.UA_WINDOWS_10_EDGE,
            string withKey = "WowThisIsSuchASecureKeyICantBelieveIt")
        {
            Mock<IOptions<JwtSettings>> options = GenerateOptionsWithKey(withKey);
            Mock<IRemoteIpAddressService> ipService = GenerateMockRemoteIpAddressService(withIpAddress);
            IUserAgentService uaService = GenerateUserAgentService(withUserAgent);
            TokenEncryptionSettings encryptionSettings = new()
            {
                UseEncryption = true,
                Passphrase = "Hello"
            };
            
            TokenGenerationService<GenerateUserToken> service = new(options.Object, ipService.Object, 
                uaService, Options.Create(encryptionSettings));

            return service;
        }
        
        public static string GenerateUserToken(string withIpAddress, 
            string withUserAgent = Constants.UA_WINDOWS_10_EDGE,
            string userId = "SomeUserId", 
            string yapilyUserId = "SomeYapilyUserId", 
            string entityId = "SomeEntityId",
            AirslipUserType airslipUserType = AirslipUserType.Standard)
        {
            ITokenGenerationService<GenerateUserToken> service = CreateTokenGenerationService<GenerateUserToken>
                (withIpAddress, "",  withUserAgent: withUserAgent);
            
            GenerateUserToken apiTokenKey = new(entityId, airslipUserType, userId,
                yapilyUserId);
            
            return service.GenerateNewToken(apiTokenKey).TokenValue;
        }
        
        public static ITokenGenerationService<TTokenType> CreateTokenGenerationService<TTokenType>(string withIpAddress, 
            string withToken, string withKey = "WowThisIsSuchASecureKeyICantBelieveIt",
            ClaimsPrincipal withClaimsPrincipal = null,
            string withUserAgent = Constants.UA_WINDOWS_10_EDGE) where TTokenType : IGenerateToken
        {
            Mock<IOptions<JwtSettings>> options = GenerateOptionsWithKey(withKey);
            Mock<IRemoteIpAddressService> ipService = GenerateMockRemoteIpAddressService(withIpAddress);
            IUserAgentService userAgentService = GenerateUserAgentService(withUserAgent);
            TokenEncryptionSettings encryptionSettings = new()
            {
                UseEncryption = true,
                Passphrase = "Hello"
            };
            TokenGenerationService<TTokenType> service = 
                new(options.Object, ipService.Object, userAgentService, Options.Create(encryptionSettings));

            return service;
        }
        
        public static ITokenDecodeService<TTokenType> CreateTokenDecodeService<TTokenType>(string withToken, 
            TokenType tokenType, ClaimsPrincipal withClaimsPrincipal = null) where TTokenType : IDecodeToken, new()
        {
            Mock<IHttpContextAccessor> contextAccessor = ContextHelpers.GenerateContext(withToken, tokenType, 
                withClaimsPrincipal: withClaimsPrincipal);
            IClaimsPrincipalLocator claimsPrincipalLocator = new HttpContextPrincipalLocator(contextAccessor.Object);
            IHttpContentLocator httpHeaderLocator = new HttpContextContentLocator(contextAccessor.Object);
            TokenEncryptionSettings encryptionSettings = new()
            {
                UseEncryption = true,
                Passphrase = "Hello"
            };
            TokenDecodeService<TTokenType> service = new(httpHeaderLocator, claimsPrincipalLocator, 
                Options.Create(encryptionSettings));

            return service;
        }

        public static string GenerateApiKeyToken(string withIpAddress, 
            string apiKey = "SomeApiKey", 
            string entityId = "SomeEntityId", 
            AirslipUserType airslipUserType = AirslipUserType.Merchant)
        {
            ITokenGenerationService<GenerateApiKeyToken> service = CreateTokenGenerationService<GenerateApiKeyToken>(withIpAddress, "");
            
            GenerateApiKeyToken apiTokenKey = new(
                entityId,
                apiKey,
                airslipUserType);
            
            return service.GenerateNewToken(apiTokenKey).TokenValue;
        }
        
        // QrCode Specifics
        public static string GenerateQrCodeToken( 
            string storeId = "SomeStoreId",
            string checkoutId = "SomeCheckoutId",
            string entityId = "SomeEntityId", 
            string qrCodeKey = "SomQrCodeKey", 
            string ipAddress = "",
            AirslipUserType airslipUserType = AirslipUserType.Merchant)
        {
            ITokenGenerationService<GenerateQrCodeToken> service = CreateTokenGenerationService<GenerateQrCodeToken>(ipAddress, "");
            
            GenerateQrCodeToken apiTokenKey = new(entityId,
                storeId,
                checkoutId,
                qrCodeKey,
                airslipUserType);
            
            return service.GenerateNewToken(apiTokenKey).TokenValue;
        }
    }
}