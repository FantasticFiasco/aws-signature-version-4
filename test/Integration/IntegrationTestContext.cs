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
        public IntegrationTestContext()
        {
            RegionName = Secrets.AwsRegion;
            UserCredentials = new ImmutableCredentials(
                Secrets.AwsUserWithPermissionsAccessKeyId,
                Secrets.AwsUserWithPermissionsSecretAccessKey,
                null);
            ApiGatewayUrl = Secrets.AwsApiGatewayUrl;
            S3Url = Secrets.AwsS3Url;
        }

        public string RegionName { get; }

        public string ServiceName { get; set; }

        public ImmutableCredentials UserCredentials { get; } 

        public ImmutableCredentials RoleCredentials { get; private set; }

        public string ApiGatewayUrl { get; }

        public string S3Url { get; }

        public async Task InitializeAsync() => RoleCredentials = await CreateRoleCredentialsAsync();

        public Task DisposeAsync() => Task.CompletedTask;

        private static async Task<ImmutableCredentials> CreateRoleCredentialsAsync()
        {
            var stsClient = new AmazonSecurityTokenServiceClient(
                Secrets.AwsUserWithoutPermissionsAccessKeyId,
                Secrets.AwsUserWithoutPermissionsSecretAccessKey);

            using (stsClient)
            {
                var request = new AssumeRoleRequest
                {
                    RoleArn = Secrets.AwsRoleArn,
                    RoleSessionName = "signature-version-4-integration-tests"
                };

                var response = await stsClient.AssumeRoleAsync(request);

                return response.Credentials.GetCredentials();
            }
        }
    }
}
