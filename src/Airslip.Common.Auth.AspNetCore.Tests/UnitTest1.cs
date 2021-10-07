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

            var encryptedString = StringCipher.Encrypt(myString, myPassphrase);
            var decryptedString = StringCipher.Decrypt(encryptedString, myPassphrase);

            decryptedString.Should().Be(myString);
        }
        
        // [Fact]
        // public void Decrypt_fails_with_wrong_passphrase()
        // {
        //     var myString = "Hello I'm a string";
        //     var myPassphrase = CommonFunctions.GetId();
        //
        //     var encryptedString = StringCipher.Encrypt(myString, myPassphrase);
        //
        //     StringCipher.Invoking(y => y.Decrypt(myString, "I'm Wroing"))
        //         .Should()
        //         .Throw<ArgumentException>();
        // }
    }
}