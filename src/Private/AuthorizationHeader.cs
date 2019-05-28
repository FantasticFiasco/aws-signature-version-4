using System;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Auth;
using Amazon.Util;

namespace AwsSignatureVersion4.Private
{
    /// <summary>
    /// Building the authorization header is one of the steps in the Signature Version 4 process. For
    /// more information, please read
    /// https://docs.aws.amazon.com/general/latest/gr/sigv4-add-signature-to-request.html.
    /// </summary>
    public static class AuthorizationHeader
    {
        public static string Build(
            DateTime now,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials,
            string signedHeaders,
            string credentialScope,
            string stringToSign)
        {
            // The following pseudocode shows the construction of the Authorization header value.
            //
            //   <algorithm> Credential=<access key id>/<credential scope>, SignedHeaders=<signed headers>, Signature=<signature>
            //
            // Note the following:
            //
            // - There is no comma between the algorithm and Credential. However, the SignedHeaders
            //   and Signature are separated from the preceding values with a comma.
            // - The Credential value starts with the access key id, which is followed by a forward
            //   slash (/), which is followed by the credential scope. The secret access key is
            //   used to derive the signing key for the signature, but is not included in the
            //   signing information sent in the request.
            //
            // To derive your signing key, use your secret access key to create a series of hash-
            // based message authentication codes (HMACs).
            //
            // Note that the date used in the hashing process is in the format YYYYMMDD (for
            // example, 20150830), and does not include the time.
            var signingKey = AWS4Signer.ComposeSigningKey(
                credentials.SecretKey,
                regionName,
                now.ToIso8601BasicDate(),
                serviceName);

            // Calculate the signature. To do this, use the signing key that you derived and the
            // string to sign as inputs to the keyed hash function. After you calculate the
            // signature, convert the binary value to a hexadecimal representation.
            var hash = AWS4Signer.ComputeKeyedHash(SigningAlgorithm.HmacSHA256, signingKey, stringToSign);
            var signature = AWSSDKUtils.ToHex(hash, true);

            return $"{AWS4Signer.AWS4AlgorithmTag} Credential={credentials.AccessKey}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={signature}";
        }
    }
}
