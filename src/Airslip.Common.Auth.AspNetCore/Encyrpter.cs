using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Airslip.Common.Auth.AspNetCore
{
    public static class StringCipher
    {
        private const int KeySize = 128;
        private const int KeySizeBytes = KeySize / 8;

        public static string Encrypt(string plainText, string passPhrase, int iterations = 1000)
        {
            byte[] keyBytes;
            byte[] saltBytes = GenerateRandomBytes();
            
            using RijndaelManaged rijAlg = new();
            using Rfc2898DeriveBytes password = new(passPhrase, saltBytes, iterations);
            keyBytes = password.GetBytes(KeySizeBytes);

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(keyBytes, rijAlg.IV);

            // Create the streams used for encryption.
            using MemoryStream msEncrypt = new();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
            }
            
            byte[] encrypted = msEncrypt.ToArray();
            byte[] cipherTextBytes = rijAlg.IV;
            cipherTextBytes = cipherTextBytes.Concat(saltBytes).ToArray();
            cipherTextBytes = cipherTextBytes.Concat(encrypted).ToArray();
                        
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string cipherText, string passPhrase, int iterations = 1000)
        {
            byte[] cipherTextBytesWithIv = Convert.FromBase64String(cipherText);
            byte[] ivStringBytes = cipherTextBytesWithIv.Take(KeySizeBytes).ToArray();
            byte[] saltBytes = cipherTextBytesWithIv.Skip(KeySizeBytes).Take(KeySizeBytes).ToArray();
            byte[] cipherTextBytes = cipherTextBytesWithIv.Skip(KeySizeBytes * 2).Take(cipherTextBytesWithIv.Length - KeySizeBytes * 2).ToArray();

            using Rfc2898DeriveBytes password = new(passPhrase, saltBytes, iterations);
            byte[] keyBytes = password.GetBytes(KeySizeBytes);
                
            using RijndaelManaged rijAlg = new();
            rijAlg.IV = ivStringBytes;

            ICryptoTransform decryptor = rijAlg.CreateDecryptor(keyBytes, rijAlg.IV);

            using MemoryStream msDecrypt = new(cipherTextBytes);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);

            string plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }
        
        private static byte[] GenerateRandomBytes()
        {
            byte[] randomBytes = new byte[KeySizeBytes];
            using RNGCryptoServiceProvider rngCsp = new();
            rngCsp.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}