# Common Classes and Frameworks

[![Dotnet Build with SonarCloud](https://github.com/airslip-ltd/common/actions/workflows/dotnet-build-sonarcloud.yml/badge.svg)](https://github.com/airslip-ltd/common/actions/workflows/dotnet-build-sonarcloud.yml)

## Api Key Authentication Usage

Api Key authentication has been added to easily allow lightweight authentication to integrations.

For ease of use this has been placed in a common package and is now available for use in any API based project that is designed to be interacted with by a machine.

### Getting Started

To get started, install the Common Auth package into your API project:

    Install-Package Airslip.Common.Auth

The add the following line in ConfigureServices:

    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddAirslipJwtAuth(Configuration, AuthType.ApiKey);
        ...
    }

This will configure the minimum required services to get started.

For requests using ApiKeys, the generated value must be provided in a HttpHeader of:

    x-api-key: <your api key>

### Using Api Key Tokens in your app

The Api Key Token is available by injecting the appropriate `ITokenService` into your implementing class, this is done through constructor injection. To use the Api Keys inject the following into your class:

    ITokenService<ApiKeyToken, GenerateApiKeyToken> myTokenService

Once you have this service you can retrieve the current token using the following function:

    ApiKeyToken token = myTokenService.GetCurrentToken();

And thats it!

## Example Usage

    private ITokenService<ApiKeyToken, GenerateApiKeyToken> _tokenService;

    public MyClass(ITokenService<ApiKeyToken, GenerateApiKeyToken> tokenService)
    {
        _tokenService = tokenService;
    }

    public void MyFunction() {
        ApiKeyToken myToken = _tokenService.GetCurrentToken();

        // myToken.AirslipUserType
        // myToken.EntityId
        // etc ...
    }

## ApiKeyToken Properties

    ApiKeyToken.ApiKey: string
    ApiKeyToken.EntityId: string
    ApiKeyToken.AirslipUserType: enum-AirslipUserType
    ApiKeyToken.CorrelationId: string
    ApiKeyToken.IpAddress: string
