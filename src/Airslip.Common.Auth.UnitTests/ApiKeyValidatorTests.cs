using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.UnitTests.Helpers;
using FluentAssertions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class ApiKeyValidatorTests
    {
        [Fact]
        public async Task Can_get_claims_principal_from_api_key_token()
        {
            string newToken = HelperFunctions.GenerateApiKeyToken("10.0.0.0");

            ApiKeyValidator apiKeyValidator = HelperFunctions.GenerateApiKeyValidator();

            ClaimsPrincipal claimsPrincipal = await apiKeyValidator.GetClaimsPrincipalFromApiKeyToken(newToken);

            claimsPrincipal.Should().NotBeNull();
            claimsPrincipal?.Claims.Count().Should().BeGreaterThan(0);
        }
        
        [Fact]
        public async Task Fails_with_invalid_api_key_token()
        {
            ApiKeyValidator apiKeyValidator = HelperFunctions.GenerateApiKeyValidator();

            await apiKeyValidator.Invoking(y => y.GetClaimsPrincipalFromApiKeyToken("I am an invalid token"))
                .Should()
                .ThrowAsync<ArgumentException>()
                .WithParameterName("tokenValue");
        }
    }
}