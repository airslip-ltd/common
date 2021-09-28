using Airslip.Common.Monitoring.Interfaces;
using Airslip.Common.Monitoring.Models;
using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Airslip.Common.Monitoring.Implementations.Checks
{
    public class ApiConnectionCheck : IHealthCheck
    {
        private readonly ILogger _logger;
        private readonly PublicApiSettings _settings;

        public ApiConnectionCheck(IOptions<PublicApiSettings> settings, ILogger logger)
        {
            _logger = logger;
            _settings = settings.Value;
        }
        
        public async Task<HealthCheckResults> Execute()
        {
            List<HealthCheckResult> results = new();
            List<PublicApiSetting> apiToCheck = new();
            
            if (_settings.BankTransactions != null) apiToCheck.Add(_settings.BankTransactions);
            if (_settings.MerchantDatabase != null) apiToCheck.Add(_settings.MerchantDatabase);
            if (_settings.MerchantTransactions != null) apiToCheck.Add(_settings.MerchantTransactions);
            if (_settings.Identity != null) apiToCheck.Add(_settings.Identity);

            foreach (var api in apiToCheck)
            {
                // Make an assumption the API has had the heartbeat endpoint added
                var uri = $"{api.BaseUri}/{api.UriSuffix ?? ""}/v1/heartbeat/ping";
                try
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
                    request.Timeout = 5000;
                    HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();

                    results.Add(checkStatusCode(response, uri));
                }
                catch (WebException we)
                {
                    if (we.Response is HttpWebResponse response)
                    {
                        results.Add(checkStatusCode(response, uri, we));
                    }
                    else
                    {
                        results.Add(new HealthCheckResult(nameof(ApiConnectionCheck), uri, false,
                            we.Message));
                    }
                }
                catch (Exception? ee) {
                    results.Add(new HealthCheckResult(nameof(ApiConnectionCheck), uri, false,
                        ee.Message));
                }
            }

            return new HealthCheckResults(results);
        }

        private HealthCheckResult checkStatusCode(HttpWebResponse response, string uri, Exception? ee = null)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new HealthCheckResult(nameof(ApiConnectionCheck), uri, false,
                    $"Incorrect status code returned calling {uri}, received {response.StatusCode}");
            }
            else
            {
                return new HealthCheckResult(nameof(ApiConnectionCheck), uri, ee == null, 
                    ee?.Message ?? "");
            }
        }
    }
}