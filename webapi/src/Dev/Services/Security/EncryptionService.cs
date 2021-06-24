using Dev.Core;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dev.Services.Security
{
    public class EncryptionService : IEncryptionService
    {
        #region Utilities

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using var ms = new MemoryStream();
            // using (var cs = new CryptoStream(ms, new  TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
            // {
            //     var toEncrypt = Encoding.Unicode.GetBytes(data);
            //     cs.Write(toEncrypt, 0, toEncrypt.Length);
            //     cs.FlushFinalBlock();
            // }

            using (var cs = new CryptoStream(ms, TripleDES.Create().CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                var toEncrypt = Encoding.Unicode.GetBytes(data);
                cs.Write(toEncrypt, 0, toEncrypt.Length);
                cs.FlushFinalBlock();
            }


            return ms.ToArray();
        }

        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using var ms = new MemoryStream(data);
            //using var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read);
            using var cs = new CryptoStream(ms, TripleDES.Create().CreateDecryptor(key, iv), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.Unicode);
            return sr.ReadToEnd();
        }

        #endregion
        public string CreatePasswordHash(string password, string saltKey, string passwordFormat)
        {
            return HashHelper.CreateHash(Encoding.UTF8.GetBytes(string.Concat(password, saltKey)), passwordFormat);
        }

        public string CreateSaltKey(int size)
        {
            //generate a cryptographic random number
            //using var provider = new RNGCryptoServiceProvider();
            // var buff = new byte[size];
            // provider.GetBytes(buff);
            var buff =  RandomNumberGenerator.GetBytes(size);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        public string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = SecurityDefaults.EncryptionKey;

            // using var provider = new TripleDESCryptoServiceProvider
            // {
            //     Key = Encoding.ASCII.GetBytes(encryptionPrivateKey[0..16]),
            //     IV = Encoding.ASCII.GetBytes(encryptionPrivateKey[8..16])
            // };]
            TripleDES provider = TripleDES.Create();
            provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey[0..16]);
            provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey[8..16]);

            var buffer = Convert.FromBase64String(cipherText);
            return DecryptTextFromMemory(buffer, provider.Key, provider.IV);
        }

        public string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = SecurityDefaults.EncryptionKey;

            // using var provider = new TripleDESCryptoServiceProvider
            // {
            //     Key = Encoding.ASCII.GetBytes(encryptionPrivateKey[0..16]),
            //     IV = Encoding.ASCII.GetBytes(encryptionPrivateKey[8..16])
            // };
            TripleDES provider = TripleDES.Create();
            provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey[0..16]);
            provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey[8..16]);

            var encryptedBinary = EncryptTextToMemory(plainText, provider.Key, provider.IV);
            return Convert.ToBase64String(encryptedBinary);
        }
    }
}
