using Airslip.Common.Types;
using FluentAssertions;
using System;
using Xunit;

namespace Airslip.Common.Auth.AspNetCore.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Can_encrypt_and_decrypt()
        {
            var myString = "Hello I'm a string";
            var myPassphrase = CommonFunctions.GetId();
            var iterations = 1000;

            var encryptedString = StringCipher.Encrypt(myString, iterations, myPassphrase);

            var decryptedString = StringCipher.Decrypt(encryptedString, iterations, myPassphrase);

            decryptedString.Should().Be(myString);


        }
    }
}