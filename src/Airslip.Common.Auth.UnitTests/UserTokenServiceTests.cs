using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.UnitTests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class UserTokenServiceTests
    {
        [Fact]
        public void Fails_with_invalid_key()
        {
            UserTokenService service = HelperFunctions.GenerateUserTokenService("", "", "", "Insecure Key");

            GenerateUserToken apiTokenKey = new("SomeUserId",
                "SomeYapilyUserId", 
                "Some Identity");
            
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
            const string identity = "MyIdentity";
            
            string newToken = HelperFunctions.GenerateUserToken(ipAddress,
                userId: userId,
                yapilyUserId: yapilyUserId,
                identity: identity,
                withUserAgent: Constants.UA_APPLE_IPHONE_XR_SAFARI);

            UserTokenService service = HelperFunctions.
                GenerateUserTokenService("", newToken, 
                    withUserAgent: Constants.UA_APPLE_IPHONE_XR_SAFARI);
            
            Tuple<UserToken, IEnumerable<Claim>> decodedToken = service.DecodeExistingToken(newToken);

            decodedToken.Should().NotBeNull();

            decodedToken.Item1.IpAddress.Should().Be(ipAddress);
            decodedToken.Item1.UserId.Should().Be(userId);
            decodedToken.Item1.YapilyUserId.Should().Be(yapilyUserId);
            decodedToken.Item1.Identity.Should().Be(identity);
            decodedToken.Item1.UserAgent.Should().Be(Constants.UA_APPLE_IPHONE_XR_SAFARI_MATCH);
        }
                
        [Fact]
        public void Can_generate_new_token_with_claims()
        {
            UserTokenService service = HelperFunctions.GenerateUserTokenService("10.0.0.1", "");

            List<Claim> claims = new()
            {
                new Claim("Name", "Value")
            };

            string newToken = service.GenerateNewToken(claims);
            
            newToken.Should().NotBeNullOrWhiteSpace();
        }
    }
}