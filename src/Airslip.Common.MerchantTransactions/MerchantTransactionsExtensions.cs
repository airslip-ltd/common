using Airslip.Common.Types.Configuration;
using Airslip.Common.Utilities.Extensions;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddMerchantTransactions<TSource>(
            this IServiceCollection services,
            IConfiguration configuration)
        where TSource : class
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

                    HttpClient httpClient = httpClientFactory
                        .CreateClient(nameof(GeneratedRetailerApiV1Client));

                    GeneratedRetailerApiV1Client client = new(httpClient)
                    {
                        BaseUrl = merchantTransactionsSettings.ToBaseUri()
                    };
                    client.SetApiKeyToken(merchantTransactionsSettings.ApiKey);
                    
                    return client;
                })
                .AddScoped<IMerchantIntegrationService<TSource>, MerchantIntegrationService<TSource>>()
                .AddHttpClient<GeneratedRetailerApiV1Client>(nameof(GeneratedRetailerApiV1Client))
                .AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            return services;
        }
    }
}