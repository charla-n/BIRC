using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace BIRC.Shared.Utils
{
    public class Encryption
    {
        private static readonly string descriptor = "LOCAL=user";
        private static readonly BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;

        public async static Task<string> Protect(string data)
        {
            DataProtectionProvider provider;
            IBuffer unprotectedBuffer;
            IBuffer protectedBuffer;

            if (data == null)
                return null;
            provider = new DataProtectionProvider(descriptor);
            unprotectedBuffer = CryptographicBuffer.ConvertStringToBinary(data, encoding);
            if (unprotectedBuffer == null)
                return null;
            protectedBuffer = await provider.ProtectAsync(unprotectedBuffer).AsTask().ConfigureAwait(false);
            return CryptographicBuffer.EncodeToBase64String(protectedBuffer);
        }

        public async static Task<string> UnProtect(string data)
        {
            DataProtectionProvider provider;
            IBuffer unprotectedBuffer;
            IBuffer protectedBuffer;

            if (data == null)
                return null;
            provider = new DataProtectionProvider(descriptor);
            protectedBuffer = CryptographicBuffer.DecodeFromBase64String(data);
            unprotectedBuffer = await provider.UnprotectAsync(protectedBuffer).AsTask().ConfigureAwait(false);
            return CryptographicBuffer.ConvertBinaryToString(encoding, unprotectedBuffer);
        }
    }
}
