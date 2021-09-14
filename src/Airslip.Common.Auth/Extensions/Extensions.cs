using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Handlers;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
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
        /// <summary>
        /// Add standard JWT authentication used across the Airslip API estate. Assumes
        /// application settings are available in the format of:
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
        public static AuthenticationBuilder? AddAirslipJwtAuth(this IServiceCollection services, IConfiguration configuration, 
            AuthType authType = AuthType.User, string withEnvironment = "")
        {
            AuthenticationBuilder? result = null;
            
            services
                .AddScoped<IRemoteIpAddressService, RemoteIpAddressService>()
                .AddScoped<IUserAgentService, UserAgentService>()
                .AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .AddAuthorization();

            if (authType.InList(AuthType.User, AuthType.All))
            {
                result = services
                    .AddScoped<ITokenService<UserToken, GenerateUserToken>, UserTokenService>()
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
            }
            
            if (authType.InList(AuthType.ApiKey, AuthType.All))
            {
                result = services
                    .AddSingleton<IApiKeyRequestHandler, ApiKeyRequestHandler>()
                    .AddScoped<ITokenService<ApiKeyToken, GenerateApiKeyToken>, ApiKeyTokenService>()
                    .AddScoped<ITokenValidator<ApiKeyToken, GenerateApiKeyToken>, TokenValidator<ApiKeyToken, GenerateApiKeyToken>>()
                    .AddAuthentication(ApiKeyAuthenticationSchemeOptions.ApiKeyScheme)
                    .AddApiKeyAuth(opt =>
                    {
                        opt.Environment = withEnvironment;
                    });
            }

            return result;
        }

        /// <summary>
        /// Add QR Code authentication. Assumes application settings are available in the format of:
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
        /// <param name="withEnvironment">The name of the environment which will be used for validating QR Codes</param>
        /// <returns>The updated service collection</returns>
        public static AuthenticationBuilder? AddAirslipQrCodeAuth(this IServiceCollection services, 
            IConfiguration configuration, string withEnvironment = "")
        {
            AuthenticationBuilder? result = null;
            
            result = services
                .AddSingleton<IQrCodeRequestHandler, QrCodeRequestHandler>()
                .AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .AddAuthorization()
                .AddScoped<ITokenService<QrCodeToken, GenerateQrCodeToken>, QrCodeTokenService>()
                .AddScoped<ITokenValidator<QrCodeToken, GenerateQrCodeToken>, TokenValidator<QrCodeToken, GenerateQrCodeToken>>()
                .AddAuthentication(QrCodeAuthenticationSchemeOptions.QrCodeAuthScheme)
                .AddQrCodeAuth(opt =>
                {
                    opt.Environment = withEnvironment;
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