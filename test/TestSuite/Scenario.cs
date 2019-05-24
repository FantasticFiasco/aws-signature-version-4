using System.IO;
using System.Net.Http;
using AwsSignatureVersion4.TestSuite.Serialization;

namespace AwsSignatureVersion4.TestSuite
{
    /// <summary>
    /// A scenario is a test from the AWS Test Suite that is verifying a specific behavior of the
    /// Signature Version 4 implementation.
    /// </summary>
    public class Scenario
    {
        public Scenario(string directory)
        {
            // It's strange, but one can get the directory name with a call for the file name
            var scenarioName = Path.GetFileName(directory);
            var basePath = Path.Combine(directory, scenarioName);

            // Deserialize request
            var requestSerializer = new RequestSerializer($"{basePath}.req");
            Request = requestSerializer.Deserialize();

            // Deserialize canonical request
            var canonicalRequestSerializer = new CanonicalRequestSerializer($"{basePath}.creq");
            ExpectedCanonicalRequest = canonicalRequestSerializer.Deserialize();
            ExpectedSignedHeaders = canonicalRequestSerializer.DeserializeSignedHeaders();

            // Deserialize string to sign
            var stringToSignSerializer = new StringToSignSerializer($"{basePath}.sts");
            ExpectedStringToSign = stringToSignSerializer.Deserialize();
            ExpectedCredentialScope = stringToSignSerializer.DeserializeCredentialScope();

            // Deserialize authorization header
            var authorizationHeaderSerializer = new StringContentSerializer($"{basePath}.authz");
            ExpectedAuthorizationHeader = authorizationHeaderSerializer.Deserialize();
        }

        public HttpRequestMessage Request { get; }

        public string ExpectedCanonicalRequest { get; }

        public string ExpectedSignedHeaders { get; }

        public string ExpectedStringToSign { get; }

        public string ExpectedCredentialScope { get; }

        public string ExpectedAuthorizationHeader { get; }
    }
}
