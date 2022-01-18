using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Functions.Extensions;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Functions.Tests.Helpers;
using Airslip.Common.Auth.Models;
using Airslip.Common.Testing;
using Airslip.Common.Types.Enums;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.Functions.Tests;

public class ApiRequestAuthServiceTests
{
    [Fact]
    public async Task Can_validate_named_function()
    {
        const string ipAddress = "10.0.0.0";
        const string apiKey = "MyNewApiKey";
        const string entityId = "SomeEntity";
        const AirslipUserType airslipUserType = AirslipUserType.Merchant;

        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        IConfiguration config = configurationBuilder.Build();

        OptionsMock.InitialiseConfiguration("");

        IServiceCollection services = new ServiceCollection();
        services
            .AddAirslipFunctionAuth(config)
            .AddNamedAccessRights("SomeFunction", AirslipUserType.Merchant,"SomeEntity");
        IServiceProvider provider = services.BuildServiceProvider();

        string newToken = HelperFunctions.GenerateApiKeyToken(ipAddress, apiKey,
            entityId, airslipUserType);

        Mock<FunctionContext> context = new();
        context.SetupProperty(c => c.InstanceServices, provider);
        Mock<HttpRequestData> mockRequestData = new(context.Object);
        HttpHeadersCollection headerCollection = new() {{AirslipSchemeOptions.ApiKeyHeaderField, newToken}};
        mockRequestData.Setup(data => data.Headers).Returns(headerCollection);
        mockRequestData.Setup(data => data.Url).Returns(new Uri("https://www.google.com"));
        
        IApiRequestAuthService authRequestService = provider.GetService<IApiRequestAuthService>() 
            ?? throw new NotImplementedException();

        KeyAuthenticationResult authResult = await authRequestService
            .Handle("SomeFunction", mockRequestData.Object);

        authResult.AuthResult.Should().Be(AuthResult.Success);
    }
}