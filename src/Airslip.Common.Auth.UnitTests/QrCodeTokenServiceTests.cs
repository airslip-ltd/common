using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Airslip.Common.Auth.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class QrCodeTokenServiceTests
    {
        [Fact]
        public void Fails_with_invalid_key()
        {
            QrCodeTokenService service = HelperFunctions.GenerateQrCodeTokenService("", "Insecure Key");

            GenerateQrCodeToken apiTokenKey = new("StoreId",
                "CheckoutId",
                "EntityId",
                AirslipUserType.Merchant,
                "SomeKey");
            
            service.Invoking(y => y.GenerateNewToken(apiTokenKey))
                .Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(JwtSettings.Key));
        }
        
        [Fact]
        public void Can_generate_new_token()
        {
            string newToken = HelperFunctions.GenerateQrCodeToken();

            newToken.Should().NotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public void Can_decode_token()
        {
            const string storeId = "001";
            const string checkoutId = "001";
            const string entityId = "MyEntityId";
            const string qrCodeKey = "SomeKey";
            const AirslipUserType airslipUserType = AirslipUserType.Merchant;
            
            string newToken = HelperFunctions.GenerateQrCodeToken(storeId, checkoutId, entityId, 
                qrCodeKey, airslipUserType);

            QrCodeTokenService service = HelperFunctions.
                GenerateQrCodeTokenService(newToken);
            
            Tuple<QrCodeToken, ICollection<Claim>> decodedToken = service.DecodeExistingToken(newToken);

            decodedToken.Should().NotBeNull();

            decodedToken.Item1.IpAddress.Should().Be("");
            decodedToken.Item1.StoreId.Should().Be(storeId);
            decodedToken.Item1.CheckoutId.Should().Be(checkoutId);
            decodedToken.Item1.EntityId.Should().Be(entityId);
            decodedToken.Item1.QrCodeKey.Should().Be(qrCodeKey);
            decodedToken.Item1.Environment.Should().Be(QrCodeAuthenticationSchemeOptions.ThisEnvironment);
            decodedToken.Item1.AirslipUserType.Should().Be(airslipUserType);
        }
        
        [Fact]
        public async Task Can_decode_token_from_principal()
        {
            const string storeId = "001";
            const string checkoutId = "001";
            const string entityId = "MyEntityId";
            const string qrCodeKey = "SomeKey";
            const AirslipUserType airslipUserType = AirslipUserType.Merchant;
            
            string newToken = HelperFunctions.GenerateQrCodeToken(storeId, checkoutId, entityId, 
                qrCodeKey, airslipUserType);

            // Prepare test data...
            TokenValidator<QrCodeToken, GenerateQrCodeToken> tempService = HelperFunctions.GenerateQrCodeValidator();
            ClaimsPrincipal claimsPrincipal = await tempService.GetClaimsPrincipalFromToken(newToken, 
                QrCodeAuthenticationSchemeOptions.QrCodeAuthScheme);

            QrCodeTokenService service = HelperFunctions.
                GenerateQrCodeTokenService(newToken, withClaimsPrincipal: claimsPrincipal);
            
            QrCodeToken currentToken = service.GetCurrentToken();

            currentToken.Should().NotBeNull();
            
            currentToken.IpAddress.Should().Be("");
            currentToken.StoreId.Should().Be(storeId);
            currentToken.CheckoutId.Should().Be(checkoutId);
            currentToken.EntityId.Should().Be(entityId);
            currentToken.QrCodeKey.Should().Be(qrCodeKey);
            currentToken.Environment.Should().Be(QrCodeAuthenticationSchemeOptions.ThisEnvironment);
            currentToken.AirslipUserType.Should().Be(airslipUserType);
        }

        [Fact]
        public void Can_generate_new_token_with_claims()
        {
            QrCodeTokenService service = HelperFunctions.GenerateQrCodeTokenService("");

            List<Claim> claims = new()
            {
                new Claim("Name", "Value")
            };

            string newToken = service.GenerateNewToken(claims).TokenValue;
            
            newToken.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Can_get_token_from_query_string()
        {
            const string storeId = "001";
            const string checkoutId = "001";
            const string entityId = "MyEntityId";
            const string qrCodeKey = "SomeKey";
            const AirslipUserType airslipUserType = AirslipUserType.Merchant;
            
            string newToken = HelperFunctions.GenerateQrCodeToken(storeId, checkoutId, entityId, 
                qrCodeKey, airslipUserType);
            
            TokenValidator<QrCodeToken, GenerateQrCodeToken> tempService = HelperFunctions.GenerateQrCodeValidator();
            QrCodeRequestHandler handler = new QrCodeRequestHandler(tempService);

            Mock<IHttpContextAccessor> context = ContextHelpers.GenerateContextWithQrCode(newToken);
            AuthenticateResult result = await handler.Handle(context.Object.HttpContext!.Request);

            result.Succeeded.Should().BeTrue();
        }
    }
}