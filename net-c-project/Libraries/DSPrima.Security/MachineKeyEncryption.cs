using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace DSPrima.Security
{
    /// <summary>
    /// Provides functionality to encrypt and decrypt strings using the Machine Key
    /// This is based upon examples found here:
    /// http://brockallen.com/2012/06/21/use-the-machinekey-api-to-protect-values-in-asp-net/
    /// http://brockallen.com/2012/06/11/cookie-based-tempdata-provider/            
    /// </summary>
    public static class MachineKeyEncryption
    {
        /// <summary>
        /// Used as the purpose for the encryption logic
        /// </summary>
        private const string MachineKeyPurpose = "ControlStringObfuscation";

        #region Encryption
        /// <summary>
        /// Encrypts the given string using the <see cref="System.Web.Security.MachineKey"/> class
        /// </summary>
        /// <param name="stringToEncrypt">The string to encrypt</param>
        /// <param name="additionalPurposes">Any additional encryption Purposes you want to add to the default MachineKey Purpose.</param>
        /// <returns>The encrypted string</returns>
        public static string Encrypt(string stringToEncrypt, params string[] additionalPurposes)
        {
            string securityString = MachineKeyEncryption.Protect(MachineKeyEncryption.Compress(stringToEncrypt), additionalPurposes);

            return securityString;
        }

        /// <summary>
        /// Decrypts the given string using the <see cref="System.Web.Security.MachineKey"/> class
        /// </summary>
        /// <param name="encryptedString">The encrypted string</param>
        /// <param name="additionalPurposes">Any additional encryption Purposes you want to add to the default MachineKey Purpose. The list of purposes has to be in the same order as they where when Encrypt was called</param>
        /// <returns>The string to decrypt</returns>
        public static string Decrypt(string encryptedString, params string[] additionalPurposes)
        {
            try
            {
                string encryptedTicket = MachineKeyEncryption.Decompress(MachineKeyEncryption.Unprotect(encryptedString, additionalPurposes));
                return encryptedTicket;
            }
            catch (CryptographicException)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Compresses a given string to an array of Bytes
        /// </summary>
        /// <param name="value">The string to compress</param>
        /// <returns>An array of bytes</returns>
        private static byte[] Compress(string value)
        {
            if (value == null) return null;

            var data = Encoding.UTF8.GetBytes(value);
            using (var input = new MemoryStream(data))
            {
                using (var output = new MemoryStream())
                {
                    using (Stream cs = new DeflateStream(output, CompressionMode.Compress))
                    {
                        input.CopyTo(cs);
                    }

                    return output.ToArray();
                }
            }
        }

        /// <summary>
        /// Decompresses the given array of bytes to a string
        /// </summary>
        /// <param name="data">The array of bytes to decompress</param>
        /// <returns>The decompressed string</returns>
        private static string Decompress(byte[] data)
        {
            if (data == null || data.Length == 0) return null;

            using (var input = new MemoryStream(data))
            {
                using (var output = new MemoryStream())
                {
                    using (Stream cs = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        cs.CopyTo(output);
                    }

                    var result = output.ToArray();
                    return Encoding.UTF8.GetString(result);
                }
            }
        }

        /// <summary>
        /// Protects (encrypts) the given array of bytes using MachineKey.Protect and the MachineKeyPurpose
        /// </summary>
        /// <param name="data">The data to protect</param>
        /// <param name="additionalPurposes">Any additional encryption Purposes you want to add to the default MachineKey Purpose.</param>
        /// <returns>The encrypted string</returns>
        private static string Protect(byte[] data, params string[] additionalPurposes)
        {
            if (data == null || data.Length == 0) return null;

            string purpose = MachineKeyEncryption.MachineKeyPurpose + (additionalPurposes.Length > 0 ? "_" + additionalPurposes.Aggregate((s1, s2) => { return s1 + "_" + s2; }) : string.Empty);

            byte[] value = MachineKey.Protect(data, purpose);
            return Convert.ToBase64String(value);
        }

        /// <summary>
        /// Unprotects (decrypts) the given string value using MachineKey.UnProtect and the MachineKeyPurpose
        /// </summary>
        /// <param name="value">The value to decrypt</param>
        /// <param name="additionalPurposes">Any additional encryption Purposes you want to add to the default MachineKey Purpose. The list of purposes has to be in the same order as they where when Protect was called</param>
        /// <returns>The decrypted data</returns>
        private static byte[] Unprotect(string value, params string[] additionalPurposes)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            string purpose = MachineKeyEncryption.MachineKeyPurpose + (additionalPurposes.Length > 0 ? "_" + additionalPurposes.Aggregate((s1, s2) => { return s1 + "_" + s2; }) : string.Empty);

            var bytes = Convert.FromBase64String(value);
            return MachineKey.Unprotect(bytes, purpose);
        }
        #endregion
    }
}
