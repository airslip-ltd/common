using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;
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

        public static Mock<IRemoteIpAddressService> GenerateRemoteIpAddressService(string ipAddress)
        {
            Mock<IRemoteIpAddressService> remoteIpAddressService = new();
            remoteIpAddressService.Setup(_ => _.GetRequestIP()).Returns(ipAddress);

            return remoteIpAddressService;
        }

        public static IUserAgentService GenerateUserAgentService(string withUserAgent = Constants.UA_WINDOWS_10_EDGE)
        {
            Mock<IHttpContextAccessor> accessor = GenerateContextWithBearerToken("", withUserAgent);
            UserAgentService userAgentService = new(accessor.Object);

            return userAgentService;
        }

        public static ApiKeyValidator GenerateApiKeyValidator()
        {
            ApiKeyValidator apiKeyValidator = new(GenerateApiKeyTokenService("", ""));

            return apiKeyValidator;
        }
        
        // User specifics
        public static Mock<IHttpContextAccessor> GenerateContextWithBearerToken(string bearerToken, 
            string userAgent = Constants.UA_WINDOWS_10_EDGE,
            ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            
            context.Request.Headers["Authorization"] = $"Bearer {bearerToken}";
            context.Request.Headers["User-Agent"] = userAgent;
            if (withClaimsPrincipal != null) context.User = withClaimsPrincipal;
            
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            return mockHttpContextAccessor;
        }
        
        public static UserTokenService GenerateUserTokenService(string withIpAddress, string withBearerToken, 
            string withUserAgent = Constants.UA_WINDOWS_10_EDGE,
            string withKey = "WowThisIsSuchASecureKeyICantBelieveIt",
            ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IOptions<JwtSettings>> options = GenerateOptionsWithKey(withKey);
            Mock<IHttpContextAccessor> contextAccessor = GenerateContextWithBearerToken(withBearerToken, withUserAgent, withClaimsPrincipal);
            Mock<IRemoteIpAddressService> ipService = GenerateRemoteIpAddressService(withIpAddress);
            IUserAgentService uaService = GenerateUserAgentService(withUserAgent);
            
            UserTokenService service = new(contextAccessor.Object, uaService,
                ipService.Object, options.Object);

            return service;
        }
        
        public static string GenerateUserToken(string withIpAddress, 
            DateTime? expiresTime = null,
            string withUserAgent = Constants.UA_WINDOWS_10_EDGE,
            string userId = "SomeUserId", 
            string yapilyUserId = "SomeYapilyUserId", 
            string identity = "SomeIdentity")
        {
            UserTokenService service = GenerateUserTokenService(withIpAddress, "", withUserAgent);
            
            GenerateUserToken apiTokenKey = new(userId,
                yapilyUserId, 
                identity);
            
            return service.GenerateNewToken(apiTokenKey, expiresTime);
        }
        
        // ApiKey Specifics
        public static Mock<IHttpContextAccessor> GenerateContextWithApiKey(string apiKey, ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            
            context.Request.Headers[ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField] = apiKey;
            if (withClaimsPrincipal != null) context.User = withClaimsPrincipal;
            
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            return mockHttpContextAccessor;
        }
        
        public static ApiKeyTokenService GenerateApiKeyTokenService(string withIpAddress, string withApiKey, 
            string withKey = "WowThisIsSuchASecureKeyICantBelieveIt",
            ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IOptions<JwtSettings>> options = GenerateOptionsWithKey(withKey);
            Mock<IHttpContextAccessor> contextAccessor = GenerateContextWithApiKey(withApiKey, withClaimsPrincipal);
            Mock<IRemoteIpAddressService> ipService = GenerateRemoteIpAddressService(withIpAddress);
            
            ApiKeyTokenService service = new(contextAccessor.Object,
                ipService.Object, options.Object);

            return service;
        }

        public static string GenerateApiKeyToken(string withIpAddress, 
            DateTime? expiresTime = null,
            string apiKey = "SomeApiKey", 
            string entityId = "SomeEntityId", 
            ApiKeyUsageType apiKeyUsageType = ApiKeyUsageType.Merchant)
        {
            ApiKeyTokenService service = GenerateApiKeyTokenService(withIpAddress, "");
            
            GenerateApiKeyToken apiTokenKey = new(apiKey,
                entityId, 
                apiKeyUsageType);
            
            return service.GenerateNewToken(apiTokenKey, expiresTime);
        }
    }
}