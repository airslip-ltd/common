using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.UnitTests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class UserTokenServiceTests
    {
        [Fact]
        public void Fails_with_invalid_key()
        {
            ITokenGenerationService<GenerateUserToken> service = HelperFunctions
                .CreateTokenGenerationService<GenerateUserToken>("", "", "Insecure Key");

            GenerateUserToken apiTokenKey = new("Some Entity",
                AirslipUserType.Standard, 
                "SomeUserId",
                "SomeYapilyUserId");
            
            service.Invoking(y => y.GenerateNewToken(apiTokenKey))
                .Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(JwtSettings.Key));
        }
        
        [Fact]
        public void Can_generate_new_token_with_ip()
        {
            string newToken = HelperFunctions.GenerateUserToken("10.0.0.0");

            newToken.Should().NotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public void Can_generate_new_token_with_null_ip()
        {
            string newToken = HelperFunctions.GenerateUserToken(null);

            newToken.Should().NotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public void Can_decode_token()
        {
            const string ipAddress = "10.0.0.0";
            const string userId = "MyUserId";
            const string yapilyUserId = "MyYapilyUserId";
            const string entityId = "MyIdentity";
            const AirslipUserType airslipUserType = AirslipUserType.Standard;
            
            string newToken = HelperFunctions.GenerateUserToken(ipAddress,
                userId: userId,
                yapilyUserId: yapilyUserId,
                entityId: entityId,
                airslipUserType: airslipUserType,
                withUserAgent: Constants.UA_APPLE_IPHONE_XR_SAFARI);

            ITokenDecodeService<UserToken> service = HelperFunctions.
                CreateTokenDecodeService<UserToken>("<irrelevant token>", TokenType.BearerToken);
            
            Tuple<UserToken, ICollection<Claim>> decodedToken = service.DecodeExistingToken(newToken);

            decodedToken.Should().NotBeNull();

            decodedToken.Item1.IpAddress.Should().Be(ipAddress);
            decodedToken.Item1.UserId.Should().Be(userId);
            decodedToken.Item1.YapilyUserId.Should().Be(yapilyUserId);
            decodedToken.Item1.EntityId.Should().Be(entityId);
            decodedToken.Item1.AirslipUserType.Should().Be(airslipUserType);
            decodedToken.Item1.UserAgent.Should().Be(Constants.UA_APPLE_IPHONE_XR_SAFARI_MATCH);
        }
                
        [Fact]
        public void Can_generate_new_token_with_claims()
        {
            ITokenGenerationService<GenerateUserToken> service = HelperFunctions.
                CreateTokenGenerationService<GenerateUserToken>("10.0.0.1", "");

            List<Claim> claims = new()
            {
                new Claim("Name", "Value")
            };

            string newToken = service.GenerateNewToken(claims).TokenValue;
            
            newToken.Should().NotBeNullOrWhiteSpace();
        }
    }
}