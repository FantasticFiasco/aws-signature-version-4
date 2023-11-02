using System;
using System.IO;
using System.Net.Http;
using Amazon;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration;
using AwsSignatureVersion4.Private;

namespace AwsSignatureVersion4.TestSuite.Fixtures
{
    public class TestSuiteFixture
    {
        public RegionEndpoint Region => Secrets.Aws.Region;

        public string ServiceName => "service";

        public DateTime UtcNow => new(2015, 8, 30, 12, 36, 00, DateTimeKind.Utc);

        public ImmutableCredentials ImmutableCredentials => new(
            "AKIDEXAMPLE",
            "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY",
            null);

        public Scenario LoadScenario(params string[] scenarioName)
        {
            var scenarioPath = Path.Combine(
                "..",
                "..",
                "..",
                "TestSuite",
                "Blueprint",
                "aws-sig-v4-test-suite",
                Path.Combine(scenarioName));

            return new Scenario(scenarioPath);
        }

        public HttpRequestMessage RedirectRequest(HttpRequestMessage request, string to)
        {
            if (request.RequestUri == null) throw new ArgumentException("RequestUri is not set on request", nameof(request));

            // Redirect the request to a new URI
            request.RequestUri = request.RequestUri
                .ToString()
                .Replace("https://example.amazonaws.com", to)
                .ToUri();

            // The "Host" header is now invalid since we redirected the request
            request.Headers.Remove("Host");

            return request;
        }
    }
}
