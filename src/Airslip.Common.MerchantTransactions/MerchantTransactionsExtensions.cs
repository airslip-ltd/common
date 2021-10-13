using Airslip.Common.Services.AutoMapper.Extensions;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Extensions;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Net.Http;

namespace Airslip.Common.MerchantTransactions
{
    public static class MerchantTransactionsExtensions
    {
        public static IServiceCollection AddMerchantTransactions(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IMapperConfigurationExpression> mapperExpression)
        {
            services
                .Configure<PublicApiSettings>(configuration.GetSection(nameof(PublicApiSettings)))
                .AddScoped<IGeneratedRetailerApiV1Client>(provider =>
                {
                    IOptions<PublicApiSettings> apiSettings = provider.GetService<IOptions<PublicApiSettings>>()!;
                    IHttpClientFactory? httpClientFactory = provider.GetService<IHttpClientFactory>();

                    PublicApiSetting merchantTransactionsSettings =
                        apiSettings.Value.GetSettingByName("MerchantTransactions");

                    if (httpClientFactory == null)
                        throw new ArgumentException("httpClientFactory not found");

                    HttpClient? httpClient = httpClientFactory
                        .CreateClient(nameof(GeneratedRetailerApiV1Client));

                    return new GeneratedRetailerApiV1Client(httpClient)
                    {
                        BaseUrl = merchantTransactionsSettings.ToBaseUri()
                    };
                })
                .AddSingleton<IMerchantIntegrationService, MerchantIntegrationService>()
                .AddAutoMapper(mapperExpression)
                .AddHttpClient<GeneratedRetailerApiV1Client>(nameof(GeneratedRetailerApiV1Client))
                .AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            return services;
        }
    }
}