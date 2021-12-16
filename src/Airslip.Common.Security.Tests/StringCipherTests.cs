using Airslip.Common.Security.Implementations;
using FluentAssertions;
using System.Web;
using Xunit;

namespace Airslip.Common.Security.Tests
{
    public class StringCipherTests
    {
        private const string PassPhrase = "SECRET_KEY";
        
        [Fact]
        public void Can_encrypted_and_decrypt_string_value()
        {
            string str = "{\"userType\":2,\"entityId\":\"entity-id\"}";
            string encryptedString = StringCipher.Encrypt(str, PassPhrase);
            string decryptedString = StringCipher.Decrypt(encryptedString, PassPhrase);
            decryptedString.Should().Be(str);
        }
        
        [Fact]
        public void Can_encrypted_and_decrypt_string_value_for_url()
        {
            string str = "{\"userType\":2,\"entityId\":\"entity-id\"}";
            string encryptedString = StringCipher.EncryptForUrl(str, PassPhrase);
            
            // Happens automatically during API request
            string urlDecodedEncryptedString = HttpUtility.UrlDecode(encryptedString);
            
            string decryptedString = StringCipher.Decrypt(urlDecodedEncryptedString, PassPhrase);
            decryptedString.Should().Be(str);
        }
    }
}