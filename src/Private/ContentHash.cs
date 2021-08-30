using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Auth;
using Amazon.Util;

namespace AwsSignatureVersion4.Private
{
    /// <summary>
    /// Calculating the content hash is not in itself one of the steps in the Signature Version 4
    /// process, but it one of the tasks in the step to build the canonical request.
    /// </summary>
    public static class ContentHash
    {
        public static async Task<string> CalculateAsync(HttpContent? content)
        {
            // Use a hash (digest) function like SHA256 to create a hashed value from the payload
            // in the body of the HTTP or HTTPS request.
            //
            // If the payload is empty, use an empty string as the input to the hash function.
            if (content == null)
            {
                // Per performance reasons, use the pre-computed hash of an empty string from the
                // AWS SDK
                return AWS4Signer.EmptyBodySha256;
            }

            var data = await content.ReadAsByteArrayAsync();
            var hash = AWS4Signer.ComputeHash(data);
            return AWSSDKUtils.ToHex(hash, true);
        }

#if NET5_0_OR_GREATER

        public static string Calculate(HttpContent? content)
        {
            // Use a hash (digest) function like SHA256 to create a hashed value from the payload
            // in the body of the HTTP or HTTPS request.
            //
            // If the payload is empty, use an empty string as the input to the hash function.
            if (content == null)
            {
                // Per performance reasons, use the pre-computed hash of an empty string from the
                // AWS SDK
                return AWS4Signer.EmptyBodySha256;
            }

            // AWS4Signer.ComputeHash() simply calls CryptoUtilFactory.CryptoInstance.ComputeSHA256Hash(), but omits a Stream-based overload
            var stream = content.ReadAsStream();
            var hash = CryptoUtilFactory.CryptoInstance.ComputeSHA256Hash(stream);
            return AWSSDKUtils.ToHex(hash, true);
        }

#endif
    }
}
