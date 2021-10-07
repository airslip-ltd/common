using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Airslip.Common.Auth.AspNetCore
{
    public static class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 32;

        public static string Encrypt(string plainText, int iterations, string passPhrase)
        {
            byte[] keyBytes;
            byte[] saltBytes = Generate256BitsOfRandomEntropy();
            byte[] ivBytes = Generate256BitsOfRandomEntropy();
            
            using RijndaelManaged rijAlg = new();
            using (Rfc2898DeriveBytes password = new(passPhrase, 
                Convert.FromBase64String(passPhrase), iterations))
            {
                keyBytes = password.GetBytes(Keysize);
            }
                
            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(keyBytes, rijAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new())
            {
                using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    byte[] encrypted = msEncrypt.ToArray();

                    var cipherTextBytes = rijAlg.IV;
                    cipherTextBytes = cipherTextBytes.Concat(encrypted).ToArray();
                        
                    return $"{rijAlg.IV.Length}:{Convert.ToBase64String(cipherTextBytes)}";
                }
            }
        }

        public static string Decrypt(string cipherText, int iterations, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            int length = int.Parse(cipherText.Substring(0, cipherText.IndexOf(":", StringComparison.Ordinal)));
            cipherText = cipherText.Substring(cipherText.IndexOf(":", StringComparison.Ordinal) + 1);
            
            var cipherTextBytesWithIv = Convert.FromBase64String(cipherText);
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithIv.Take(length).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithIv.Skip(length).Take(cipherTextBytesWithIv.Length - length).ToArray();

            string plaintext;
            using (var password = new Rfc2898DeriveBytes(passPhrase, Convert.FromBase64String(passPhrase), iterations))
            {
                byte[] keyBytes = password.GetBytes(Keysize);
                
                // Create an RijndaelManaged object
                // with the specified key and IV.
                using (RijndaelManaged rijAlg = new())
                {
                    rijAlg.IV = ivStringBytes;

                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(keyBytes, rijAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new(cipherTextBytes))
                    {
                        using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            
            return plaintext;
        }
        
        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[16]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}