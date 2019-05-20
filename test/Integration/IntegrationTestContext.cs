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
        public IntegrationTestContext()
        {
            RegionName = IntegrationTestEnvironmentVariables.AwsRegion;
            ServiceName = "execute-api";
            UserCredentials = new ImmutableCredentials(
                IntegrationTestEnvironmentVariables.AwsUserAccessKeyId,
                IntegrationTestEnvironmentVariables.AwsUserSecretAccessKey,
                null);
            ApiGatewayUrl = new Uri(IntegrationTestEnvironmentVariables.AwsApiGatewayUrl);
        }

        public string RegionName { get; }

        public string ServiceName { get; }

        public ImmutableCredentials UserCredentials { get; }

        public ImmutableCredentials RoleCredentials { get; private set; }

        public Uri ApiGatewayUrl { get; }

        public async Task InitializeAsync() => RoleCredentials = await CreateRoleCredentials();

        public Task DisposeAsync() => Task.CompletedTask;

        private static async Task<ImmutableCredentials> CreateRoleCredentials()
        {
            var stsClient = new AmazonSecurityTokenServiceClient(
                IntegrationTestEnvironmentVariables.AwsRoleAccessKeyId,
                IntegrationTestEnvironmentVariables.AwsRoleSecretAccessKey,
                (string)null);

            using (stsClient)
            {
                var request = new AssumeRoleRequest
                {
                    RoleArn = Environment.GetEnvironmentVariable("AWS_ROLE_ARN"),
                    RoleSessionName = "signature-version-4-integration-tests"
                };

                var response = await stsClient.AssumeRoleAsync(request);

                return response.Credentials.GetCredentials();
            }
        }
    }
}
