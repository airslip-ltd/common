using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Handlers;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Auth.Extensions
{
    public static class Extensions
    {
        public static string GetEnvironment(this IServiceCollection services)
        {
            return services
                .BuildServiceProvider()
                .GetService<IOptions<EnvironmentSettings>>()!
                .Value
                .EnvironmentName;
        }
        
        /// <summary>
        /// Add token generation for the specified token type.
        /// Assumes application settings are available in the format of:
        /// 
        /// appSettings.json:
        /// {
        ///     "JwtSettings": {
        ///         "Key": "Example key",
        ///         "Issuer": "Example issuer",
        ///         "Audience": "Example audience",
        ///         "ExpiresTime": "Example expiry time",
        ///         "ValidateLifetime": "true"
        ///      }
        /// }
        /// 
        /// Environment Variables:
        /// JwtSettings:Key = Example key
        /// JwtSettings:Issuer = Example issuer
        /// JwtSettings:Audience = Example audience
        /// JwtSettings:ExpiresTime = Example expiry time
        /// JwtSettings:ValidateLifetime = true
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddTokenGeneration<TTokenType>(this IServiceCollection services, 
            IConfiguration configuration) 
            where TTokenType : IGenerateToken
        {
            services
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .AddScoped<ITokenGenerationService<TTokenType>, TokenGenerationService<TTokenType>>();

            return services;
        }
        
        /// <summary>
        /// Add standard JWT authentication used across the Airslip API estate.
        /// Assumes application settings are available in the format of:
        /// 
        /// appSettings.json:
        /// {
        ///     "JwtSettings": {
        ///         "Key": "Example key",
        ///         "Issuer": "Example issuer",
        ///         "Audience": "Example audience",
        ///         "ExpiresTime": "Example expiry time",
        ///         "ValidateLifetime": "true"
        ///      }
        /// }
        /// 
        /// Environment Variables:
        /// JwtSettings:Key = Example key
        /// JwtSettings:Issuer = Example issuer
        /// JwtSettings:Audience = Example audience
        /// JwtSettings:ExpiresTime = Example expiry time
        /// JwtSettings:ValidateLifetime = true
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <param name="authType">The auth type that is supported by this Api</param>
        /// <param name="withEnvironment">The name of the environment which will be used for validating API Keys</param>
        /// <returns>The updated service collection</returns>
        public static AuthenticationBuilder? AddAirslipJwtAuth(this IServiceCollection services,  
            IConfiguration configuration, AuthType authType = AuthType.User, string? withEnvironment = null)
        {
            AuthenticationBuilder? result = null;

            
            services
                .AddScoped<IRemoteIpAddressService, RemoteIpAddressService>()
                .AddScoped<IUserAgentService, UserAgentService>()
                .AddScoped<IClaimsPrincipalLocator, HttpContextPrincipalLocator>()
                .AddScoped<IHttpHeaderLocator, HttpContextHeaderLocator>()
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .Configure<EnvironmentSettings>(configuration.GetSection(nameof(EnvironmentSettings)))
                .AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                .AddAuthorization();

            if (authType.InList(AuthType.User, AuthType.All))
            {
                result = services
                    .AddScoped<ITokenDecodeService<UserToken>, TokenDecodeService<UserToken>>()
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
            }
            
            if (authType.InList(AuthType.ApiKey, AuthType.All))
            {
                result = services
                    .AddSingleton<IApiKeyRequestHandler, ApiKeyRequestHandler>()
                    .AddScoped<ITokenDecodeService<ApiKeyToken>, TokenDecodeService<ApiKeyToken>>()
                    .AddScoped<ITokenValidator<ApiKeyToken>, TokenValidator<ApiKeyToken>>()
                    .AddAuthentication(ApiKeyAuthenticationSchemeOptions.ApiKeyScheme)
                    .AddApiKeyAuth(opt =>
                    {
                        opt.Environment = withEnvironment ?? services.GetEnvironment();
                    });
            }

            return result;
        }

        /// <summary>
        /// Add QR Code authentication.
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <param name="withEnvironment">The name of the environment which will be used for validating QR Codes</param>
        /// <returns>The updated service collection</returns>
        public static AuthenticationBuilder AddAirslipQrCodeAuth(this IServiceCollection services, 
            IConfiguration configuration, string? withEnvironment)
        {
            services
                .AddSingleton<IQrCodeRequestHandler, QrCodeRequestHandler>()
                .AddAuthorization()
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .Configure<EnvironmentSettings>(configuration.GetSection(nameof(EnvironmentSettings)))
                .AddScoped<ITokenDecodeService<QrCodeToken>, TokenDecodeService<QrCodeToken>>()
                .AddScoped<ITokenValidator<QrCodeToken>, TokenValidator<QrCodeToken>>();
            
            AuthenticationBuilder result = services
                .AddAuthentication(QrCodeAuthenticationSchemeOptions.QrCodeAuthScheme)
                .AddQrCodeAuth(opt =>
                {
                    opt.Environment = withEnvironment ?? services.GetEnvironment();
                });

            return result;
        }
        
        public static AuthenticationBuilder AddApiKeyAuth(this AuthenticationBuilder builder, Action<ApiKeyAuthenticationSchemeOptions> options)
        {
            return builder
                .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthHandler>(ApiKeyAuthenticationSchemeOptions.ApiKeyScheme, options);
        }
        
        public static AuthenticationBuilder AddQrCodeAuth(this AuthenticationBuilder builder, Action<QrCodeAuthenticationSchemeOptions> options)
        {
            return builder
                .AddScheme<QrCodeAuthenticationSchemeOptions, QrCodeAuthHandler>(QrCodeAuthenticationSchemeOptions.QrCodeAuthScheme, options);
        }
        
        public static bool IsNullOrWhitespace(this string? s)
        {
            return s == null || string.IsNullOrWhiteSpace(s);
        }

        public static List<string> SplitCsv(this string csvList)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
    }
}