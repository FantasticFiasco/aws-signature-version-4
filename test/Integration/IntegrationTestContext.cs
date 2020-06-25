using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using AwsSignatureVersion4.TestSuite;
using Xunit;

namespace AwsSignatureVersion4.Integration
{
    /// <summary>
    /// Class setting up a context that is valid when we run integration tests towards a real AWS
    /// API Gateway.
    /// </summary>
    public class IntegrationTestContext : Context, IAsyncLifetime
    {
        private readonly IntegrationTestVariables variables;

        public IntegrationTestContext()
        {
            variables = new IntegrationTestVariables();

            RegionName = variables.GetValue("AWS_REGION");
            ServiceName = "execute-api";
            UserCredentials = new ImmutableCredentials(
                variables.GetValue("AWS_USER_ACCESS_KEY_ID"),
                variables.GetValue("AWS_USER_SECRET_ACCESS_KEY"),
                null);
            ApiGatewayUrl = variables.GetValue("AWS_API_GATEWAY_URL");
        }

        public string RegionName { get; }

        public string ServiceName { get; }

        public ImmutableCredentials UserCredentials { get; }

        public ImmutableCredentials RoleCredentials { get; private set; }

        public string ApiGatewayUrl { get; }

        public async Task InitializeAsync() => RoleCredentials = await CreateRoleCredentials();

        public Task DisposeAsync() => Task.CompletedTask;

        private async Task<ImmutableCredentials> CreateRoleCredentials()
        {
            var stsClient = new AmazonSecurityTokenServiceClient(
                variables.GetValue("AWS_ROLE_ACCESS_KEY_ID"),
                variables.GetValue("AWS_ROLE_SECRET_ACCESS_KEY"));

            using (stsClient)
            {
                var request = new AssumeRoleRequest
                {
                    RoleArn = variables.GetValue("AWS_ROLE_ARN"),
                    RoleSessionName = "signature-version-4-integration-tests"
                };

                var response = await stsClient.AssumeRoleAsync(request);

                return response.Credentials.GetCredentials();
            }
        }
    }
}
