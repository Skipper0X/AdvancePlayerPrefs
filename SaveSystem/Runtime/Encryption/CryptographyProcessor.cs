using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SaveSystem.Encryption
{
    /// <summary>
    /// Basic Cryptography Operation Types.....
    /// </summary>
    public enum CryptographyType
    {
        Encrypt = 0,
        Decrypt = 1,
    }

    /// <summary>
    /// Basic Encryption/Decryption For Plain-Text........... 
    /// </summary>
    public static class CryptographyProcessor
    {
        private const string INIT_VECTOR = "pnjv89dAvud02i";
        private static readonly Cryptography Cryptography = new Cryptography(INIT_VECTOR);

        /// <summary>
        /// Process The Given Data With Respective <see cref="CryptographyType"/> & Return Data'<see cref="string"/>
        /// </summary>
        /// <param name="stringData"><see cref="string"/> Of Data To Process</param>
        /// <param name="cryptographyType"><see cref="CryptographyType"/> To Process Data..</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Process(string stringData, CryptographyType cryptographyType)
        {
            return cryptographyType switch
            {
                CryptographyType.Encrypt => Cryptography.Encrypt(stringData),
                CryptographyType.Decrypt => Cryptography.Decrypt(stringData),
                _ => throw new ArgumentOutOfRangeException(nameof(cryptographyType), cryptographyType, null)
            };
        }
    }

    internal class Cryptography : IDisposable
    {
        private readonly Aes _encryptor;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Cryptography(string key)
        {
            _encryptor = Aes.Create();
            var pdb = new Rfc2898DeriveBytes(key, new byte[]
            {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });

            _encryptor.Key = pdb.GetBytes(32);
            _encryptor.IV = pdb.GetBytes(16);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            var clearBytes = Encoding.Unicode.GetBytes(plainText);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, _encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
            }

            plainText = Convert.ToBase64String(ms.ToArray());

            return plainText;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            cipherText = cipherText.Replace(" ", "+");
            var cipherBytes = Convert.FromBase64String(cipherText);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, _encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
            }

            cipherText = Encoding.Unicode.GetString(ms.ToArray());
            return cipherText;
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() => _encryptor.Dispose();
    }
}