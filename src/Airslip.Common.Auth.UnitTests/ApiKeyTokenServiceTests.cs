using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.UnitTests.Helpers;
using FluentAssertions;
using System;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class ApiKeyTokenServiceTests
    {
        [Fact]
        public void Fails_with_invalid_key()
        {
            ApiKeyTokenService service = HelperFunctions.GenerateTokenService("", "", "Insecure Key");

            GenerateApiKeyToken apiTokenKey = new("SomeApiKey",
                "SomeEntityId", 
                ApiKeyUsageType.Merchant);
            
            service.Invoking(y => y.GenerateNewToken(apiTokenKey))
                .Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(JwtSettings.Key));
        }
        
        [Fact]
        public void Can_generate_new_token()
        {
            string newToken = HelperFunctions.GenerateApiKeyToken("10.0.0.0");

            newToken.Should().NotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public void Can_generate_new_token_with_null_ip()
        {
            string newToken = HelperFunctions.GenerateApiKeyToken(null);

            newToken.Should().NotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public void Can_decode_token()
        {
            const string ipAddress = "10.0.0.0";
            const string apiKey = "MyNewApiKey";
            const string entityId = "MyEntityId";
            const ApiKeyUsageType apiKeyUsageType = ApiKeyUsageType.Merchant;
            
            string newToken = HelperFunctions.GenerateApiKeyToken(ipAddress, null, apiKey, entityId, 
                apiKeyUsageType);

            ApiKeyTokenService service = HelperFunctions.
                GenerateTokenService("", newToken);
            
            var decodedToken = service.DecodeExistingToken(newToken);

            decodedToken.Should().NotBeNull();

            decodedToken.Item1.IpAddress.Should().Be(ipAddress);
            decodedToken.Item1.ApiKey.Should().Be(apiKey);
            decodedToken.Item1.EntityId.Should().Be(entityId);
            decodedToken.Item1.ApiKeyUsageType.Should().Be(apiKeyUsageType);
        }
    }
}