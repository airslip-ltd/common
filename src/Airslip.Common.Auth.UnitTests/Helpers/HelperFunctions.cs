using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;

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

        public static Mock<IHttpContextAccessor> GenerateContextWithApiKey(string apiKey)
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            
            context.Request.Headers[ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField] = apiKey;
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            return mockHttpContextAccessor;
        }

        public static Mock<IRemoteIpAddressService> GenerateRemoteIpAddressService(string ipAddress)
        {
            Mock<IRemoteIpAddressService> remoteIpAddressService = new();
            remoteIpAddressService.Setup(_ => _.GetRequestIP()).Returns(ipAddress);

            return remoteIpAddressService;
        }
        
        public static ApiKeyTokenService GenerateTokenService(string withIpAddress, string withApiKey, 
            string withKey = "WowThisIsSuchASecureKeyICantBelieveIt")
        {
            Mock<IOptions<JwtSettings>> options = GenerateOptionsWithKey(withKey);
            Mock<IHttpContextAccessor> contextAccessor = GenerateContextWithApiKey(withApiKey);
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
            ApiKeyTokenService service = GenerateTokenService(withIpAddress, "");
            
            GenerateApiKeyToken apiTokenKey = new(apiKey,
                entityId, 
                apiKeyUsageType);
            
            return service.GenerateNewToken(apiTokenKey, expiresTime);
        }
    }
}