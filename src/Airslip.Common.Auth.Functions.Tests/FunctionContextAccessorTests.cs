using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Functions.Implementations;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Functions.Tests.Helpers;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.Functions.Tests
{
    public class FunctionContextAccessorTests
    {
        [Fact]
        public void Can_store_principal_in_accessor()
        {
            const string ipAddress = "10.0.0.0";
            const string apiKey = "MyNewApiKey";
            const string entityId = "MyEntityId";
            const AirslipUserType airslipUserType = AirslipUserType.Merchant;
            
            string newToken = HelperFunctions.GenerateApiKeyToken(ipAddress, apiKey, 
                entityId, airslipUserType);
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var context = new Mock<FunctionContext>();
            context.SetupProperty(c => c.InstanceServices, serviceProvider);
            
            // Mock the request data...
            Mock<HttpRequestData> mockRequestData = new(context.Object);
            HttpHeadersCollection headerCollection = new();
            headerCollection.Add(AirslipSchemeOptions.ApiKeyHeaderField, newToken);
            mockRequestData.Setup(data => data.Headers).Returns(headerCollection);

            // Create the validator
            IFunctionContextAccessor functionContextAccessor = new FunctionContextAccessor();
            IHttpHeaderLocator httpHeaderLocator = new FunctionContextHeaderLocator(functionContextAccessor);
            IClaimsPrincipalLocator claimsPrincipalLocator =
                new FunctionContextPrincipalLocator(functionContextAccessor);
            ITokenDecodeService<ApiKeyToken> tokenDecodeService =
                new TokenDecodeService<ApiKeyToken>(httpHeaderLocator, claimsPrincipalLocator);
            ITokenValidator<ApiKeyToken> tokenValidator = new TokenValidator<ApiKeyToken>(tokenDecodeService);

            // Create the handler
            IApiKeyRequestDataHandler handler = new ApiKeyRequestDataHandler(tokenValidator, functionContextAccessor);
            
            // Now we can validate...
            Task<KeyAuthenticationResult> valid = handler.Handle(mockRequestData.Object);

            valid.Result.AuthResult.Should().Be(AuthResult.Success);

            // Finally test we can get the current token...
            ApiKeyToken currentToken = tokenDecodeService.GetCurrentToken();

            currentToken.Should().NotBeNull();

            currentToken.IpAddress.Should().Be(ipAddress);
            currentToken.ApiKey.Should().Be(apiKey);
            currentToken.EntityId.Should().Be(entityId);
            currentToken.Environment.Should().Be(AirslipSchemeOptions.ThisEnvironment);
            currentToken.AirslipUserType.Should().Be(airslipUserType);
        }
    }
}