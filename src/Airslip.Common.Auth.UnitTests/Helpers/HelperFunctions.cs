using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
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
            Mock<IHttpContextAccessor> context = ContextHelpers.GenerateContextWithForwarder(withForwarder, withRemoteAddr);
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
            Mock<IHttpContextAccessor> accessor = ContextHelpers.GenerateContextWithBearerToken("", withUserAgent);
            UserAgentService userAgentService = new(accessor.Object);

            return userAgentService;
        }

        public static TokenValidator<ApiKeyToken, GenerateApiKeyToken> GenerateApiKeyValidator()
        {
            TokenValidator<ApiKeyToken, GenerateApiKeyToken> apiKeyValidator = 
                new(GenerateApiKeyTokenService("", ""));

            return apiKeyValidator;
        }

        public static TokenValidator<QrCodeToken, GenerateQrCodeToken> GenerateQrCodeValidator()
        {
            TokenValidator<QrCodeToken, GenerateQrCodeToken> qrCodeValidator = 
                new(GenerateQrCodeTokenService(""));

            return qrCodeValidator;
        }
        
        // User specifics
        public static UserTokenService GenerateUserTokenService(string withIpAddress, string withBearerToken, 
            string withUserAgent = Constants.UA_WINDOWS_10_EDGE,
            string withKey = "WowThisIsSuchASecureKeyICantBelieveIt",
            ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IOptions<JwtSettings>> options = GenerateOptionsWithKey(withKey);
            Mock<IHttpContextAccessor> contextAccessor = ContextHelpers.GenerateContextWithBearerToken(withBearerToken, withUserAgent, withClaimsPrincipal);
            Mock<IRemoteIpAddressService> ipService = GenerateMockRemoteIpAddressService(withIpAddress);
            IUserAgentService uaService = GenerateUserAgentService(withUserAgent);
            
            UserTokenService service = new(contextAccessor.Object, uaService,
                ipService.Object, options.Object);

            return service;
        }
        
        public static string GenerateUserToken(string withIpAddress, 
            string withUserAgent = Constants.UA_WINDOWS_10_EDGE,
            string userId = "SomeUserId", 
            string yapilyUserId = "SomeYapilyUserId", 
            string entityId = "SomeEntityId",
            AirslipUserType airslipUserType = AirslipUserType.Standard)
        {
            UserTokenService service = GenerateUserTokenService(withIpAddress, "", withUserAgent);
            
            GenerateUserToken apiTokenKey = new(userId,
                yapilyUserId, 
                entityId,
                airslipUserType);
            
            return service.GenerateNewToken(apiTokenKey).TokenValue;
        }
        
        // ApiKey Specifics
        public static ApiKeyTokenService GenerateApiKeyTokenService(string withIpAddress, string withApiKey, 
            string withKey = "WowThisIsSuchASecureKeyICantBelieveIt",
            ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IOptions<JwtSettings>> options = GenerateOptionsWithKey(withKey);
            Mock<IHttpContextAccessor> contextAccessor = ContextHelpers.GenerateContextWithApiKey(withApiKey, withClaimsPrincipal);
            Mock<IRemoteIpAddressService> ipService = GenerateMockRemoteIpAddressService(withIpAddress);
            
            ApiKeyTokenService service = new(contextAccessor.Object,
                ipService.Object, options.Object);

            return service;
        }

        public static string GenerateApiKeyToken(string withIpAddress, 
            string apiKey = "SomeApiKey", 
            string entityId = "SomeEntityId", 
            AirslipUserType airslipUserType = AirslipUserType.Merchant)
        {
            ApiKeyTokenService service = GenerateApiKeyTokenService(withIpAddress, "");
            
            GenerateApiKeyToken apiTokenKey = new(apiKey,
                entityId, 
                airslipUserType);
            
            return service.GenerateNewToken(apiTokenKey).TokenValue;
        }
        
        // QrCode Specifics
        public static QrCodeTokenService GenerateQrCodeTokenService(string withQrCode,  
            string withKey = "WowThisIsSuchASecureKeyICantBelieveIt",
            ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IOptions<JwtSettings>> options = GenerateOptionsWithKey(withKey);
            Mock<IHttpContextAccessor> contextAccessor = ContextHelpers.GenerateContextWithQrCode(withQrCode, withClaimsPrincipal);
            
            QrCodeTokenService service = new(contextAccessor.Object,
                 options.Object);

            return service;
        } 

        public static string GenerateQrCodeToken( 
            string storeId = "SomeStoreId",
            string checkoutId = "SomeCheckoutId",
            string entityId = "SomeEntityId", 
            string qrCodeKey = "SomQrCodeKey", 
            AirslipUserType airslipUserType = AirslipUserType.Merchant)
        {
            QrCodeTokenService service = GenerateQrCodeTokenService("");
            
            GenerateQrCodeToken apiTokenKey = new(storeId,
                checkoutId,
                entityId, 
                airslipUserType,
                qrCodeKey);
            
            return service.GenerateNewToken(apiTokenKey).TokenValue;
        }
    }
}