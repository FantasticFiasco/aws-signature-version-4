using System;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using AWS.SignatureVersion4.TestSuite;
using Xunit;

namespace AWS.SignatureVersion4.Integration
{
    /// <summary>
    /// Class setting up a context that is valid when we run integration tests towards a real AWS
    /// API Gateway. The values are not static but is retrieved from environment variables in the
    /// environment the tests are running in.
    /// </summary>
    public class IntegrationTestContext : Context, IAsyncLifetime
    {
        public string RegionName { get; } = IntegrationTestEnvironmentVariables.AwsRegion;

        public string ServiceName { get; } = "execute-api";

        public ImmutableCredentials UserCredentials { get; } = new ImmutableCredentials(
            IntegrationTestEnvironmentVariables.AwsUserAccessKeyId,
            IntegrationTestEnvironmentVariables.AwsUserSecretAccessKey,
            null);

        public ImmutableCredentials RoleCredentials { get; private set; }

        public Uri ApiGatewayUrl { get; } = new Uri(IntegrationTestEnvironmentVariables.AwsApiGatewayUrl);

        public async Task InitializeAsync()
        {
            var credentials = new Credentials(
                IntegrationTestEnvironmentVariables.AwsRoleAccessKeyId,
                IntegrationTestEnvironmentVariables.AwsRoleSecretAccessKey,
                null,
                DateTime.MaxValue);

            using (var client = new AmazonSecurityTokenServiceClient(credentials))
            {
                var request = new AssumeRoleRequest
                {
                    RoleArn = Environment.GetEnvironmentVariable("AWS_ROLE_ARN")
                };

                var response = await client.AssumeRoleAsync(request);

                RoleCredentials = response.Credentials.GetCredentials();
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
