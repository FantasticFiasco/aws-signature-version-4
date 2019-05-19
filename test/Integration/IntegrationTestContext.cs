using System;
using Amazon.Runtime;
using AWS.SignatureVersion4.TestSuite;

namespace AWS.SignatureVersion4.Integration
{
    /// <summary>
    /// Class setting up a context that is valid when we run integration tests towards a real AWS
    /// API Gateway. The values are not static but is retrieved from environment variables in the
    /// environment the tests are running in.
    /// </summary>
    public class IntegrationTestContext : Context
    {
        public string RegionName { get; } = Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION");

        public string ServiceName { get; } = "execute-api";

        public ImmutableCredentials UserCredentials { get; } = new ImmutableCredentials(
            Environment.GetEnvironmentVariable("AWS_USER_ACCESS_KEY_ID"),
            Environment.GetEnvironmentVariable("AWS_USER_SECRET_ACCESS_KEY"),
            null);

        public ImmutableCredentials RoleCredentials { get; } = new ImmutableCredentials(
            Environment.GetEnvironmentVariable("AWS_ROLE_ACCESS_KEY_ID"),
            Environment.GetEnvironmentVariable("AWS_ROLE_SECRET_ACCESS_KEY"),
            Environment.GetEnvironmentVariable("AWS_ROLE_SESSION_TOKEN"));

        public Uri ApiGatewayUrl { get; } = new Uri(Environment.GetEnvironmentVariable("API_GATEWAY_URL"));
    }
}
