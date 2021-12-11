using Airslip.Common.Security.Implementations;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Xunit;

namespace Airslip.Common.Security.Tests
{
    public class HmacValidatorTests
    {
        private const string SecretKey = "SECRET_KEY";
        
        [Fact]
        public void Can_decipher_a_query_string_and_match_to_a_hmac()
        {
            Dictionary<string, StringValues> queryStrings = new()
            {
                {"shop", "mystore.airslip.com"},
                {"signature", "f477a85f3ed6027735589159f9da74da"},
                {"timestamp", "1459779785"},
            };

            string hmac = "A17AC303A0E3957183F9BF58B20203BFB38AFA890FECE30BD35BECD748A10665";
            bool isValid = HmacCipher.Validate(queryStrings, hmac, SecretKey);
            isValid.Should().BeTrue();
        }
        
        [Fact]
        public void Can_decipher_an_unordered_query_string_and_match_to_a_hmac()
        {
            Dictionary<string, StringValues> queryStrings = new()
            {
                {"timestamp", "1459779785"},
                {"shop", "mystore.airslip.com"},
                {"signature", "f477a85f3ed6027735589159f9da74da"},
            };

            string hmac = "A17AC303A0E3957183F9BF58B20203BFB38AFA890FECE30BD35BECD748A10665";
            bool isValid = HmacCipher.Validate(queryStrings, hmac, SecretKey);
            isValid.Should().BeTrue();
        }
    }
}