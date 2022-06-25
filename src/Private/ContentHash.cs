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
        /// <remarks>This method has a synchronous alternative.</remarks>
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

            var contentStream = await content.ReadAsStreamAsync().ConfigureAwait(false);

            // Save current stream position
            var currentPosition = contentStream.Position;

            var hash = CryptoUtilFactory.CryptoInstance.ComputeSHA256Hash(contentStream);

            // Reset stream position
            contentStream.Position = currentPosition;

            return AWSSDKUtils.ToHex(hash, true);
        }

#if NET5_0_OR_GREATER

        /// <remarks>This method has a asynchronous alternative.</remarks>
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

            var contentStream = content.ReadAsStream();

            // Save current stream position
            var currentPosition = contentStream.Position;

            var hash = CryptoUtilFactory.CryptoInstance.ComputeSHA256Hash(contentStream);

            // Reset stream position
            contentStream.Position = currentPosition;

            return AWSSDKUtils.ToHex(hash, true);
        }

#endif
    }
}
