using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Airslip.Common.Auth.UnitTests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class ApiKeyTokenServiceTests
    {
        [Fact]
        public void Fails_with_invalid_key()
        {
            ITokenGenerationService<GenerateApiKeyToken> service = HelperFunctions
                .CreateTokenGenerationService<GenerateApiKeyToken>("", "", "Insecure Key");

            GenerateApiKeyToken apiTokenKey = new("SomeApiKey",
                "SomeEntityId", 
                AirslipUserType.Merchant);
            
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
            const AirslipUserType airslipUserType = AirslipUserType.Merchant;
            
            string newToken = HelperFunctions.GenerateApiKeyToken(ipAddress, apiKey, entityId, 
                airslipUserType);

            ITokenDecodeService<ApiKeyToken> service = HelperFunctions.
                CreateTokenDecodeService<ApiKeyToken>(newToken, TokenType.ApiKey);
            
            Tuple<ApiKeyToken, ICollection<Claim>> decodedToken = service.DecodeExistingToken(newToken);

            decodedToken.Should().NotBeNull();

            decodedToken.Item1.IpAddress.Should().Be(ipAddress);
            decodedToken.Item1.ApiKey.Should().Be(apiKey);
            decodedToken.Item1.EntityId.Should().Be(entityId);
            decodedToken.Item1.Environment.Should().Be(ApiKeyAuthenticationSchemeOptions.ThisEnvironment);
            decodedToken.Item1.AirslipUserType.Should().Be(airslipUserType);
        }
        
        [Fact]
        public async Task Can_decode_token_from_principal()
        {
            const string ipAddress = "10.0.0.0";
            const string apiKey = "MyNewApiKey";
            const string entityId = "MyEntityId";
            const AirslipUserType airslipUserType = AirslipUserType.Merchant;
            
            string newToken = HelperFunctions.GenerateApiKeyToken(ipAddress, apiKey, 
                entityId, airslipUserType);

            // Prepare test data...
            ITokenValidator<ApiKeyToken> tempService = HelperFunctions.GenerateValidator<ApiKeyToken>(TokenType.ApiKey);
            ClaimsPrincipal claimsPrincipal = await tempService.GetClaimsPrincipalFromToken(newToken, 
                ApiKeyAuthenticationSchemeOptions.ApiKeyScheme,
                ApiKeyAuthenticationSchemeOptions.ThisEnvironment);

            ITokenDecodeService<ApiKeyToken> service = HelperFunctions.
                CreateTokenDecodeService<ApiKeyToken>(newToken, TokenType.ApiKey, claimsPrincipal);
            
            ApiKeyToken currentToken = service.GetCurrentToken();

            currentToken.Should().NotBeNull();

            currentToken.IpAddress.Should().Be(ipAddress);
            currentToken.ApiKey.Should().Be(apiKey);
            currentToken.EntityId.Should().Be(entityId);
            currentToken.Environment.Should().Be(ApiKeyAuthenticationSchemeOptions.ThisEnvironment);
            currentToken.AirslipUserType.Should().Be(airslipUserType);
        }
                
        [Fact]
        public void Can_generate_new_token_with_claims()
        {
            ITokenGenerationService<GenerateApiKeyToken> service = HelperFunctions
                .CreateTokenGenerationService<GenerateApiKeyToken>("10.0.0.1", "");

            List<Claim> claims = new()
            {
                new Claim("Name", "Value")
            };

            string newToken = service.GenerateNewToken(claims).TokenValue;
            
            newToken.Should().NotBeNullOrWhiteSpace();
        }
    }
}