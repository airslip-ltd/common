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