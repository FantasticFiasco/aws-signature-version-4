using System;
using System.Net.Http;
using Amazon;
using Amazon.Runtime;

namespace AwsSignatureVersion4.Integration.ApiGateway.Fixtures
{
    public class ApiGatewayCollectionFixture : IDisposable
    {
        public ApiGatewayCollectionFixture()
        {
            HttpClient = new HttpClient();
        }

        public RegionEndpoint Region { get; } = Secrets.Aws.Region;

        public string ServiceName { get; } = "execute-api";

        public string ApiGatewayUrl { get; } = Secrets.Aws.ApiGateway.Url;

        public AWSCredentials UserCredentials { get; } = Secrets.Aws.UserWithPermissions.Credentials;

        public AWSCredentials RoleCredentials { get; } = new AssumeRoleAWSCredentials(
            Secrets.Aws.UserWithoutPermissions.Credentials,
            Secrets.Aws.Role.Arn,
            "signature-version-4-integration-tests");

        public HttpClient HttpClient { get; }

        public AWSCredentials ResolveCredentials(
            IamAuthenticationType iamAuthenticationType) =>
            iamAuthenticationType switch
            {
                IamAuthenticationType.User => UserCredentials,
                IamAuthenticationType.Role => RoleCredentials,
                _ => throw new NotImplementedException($"The authentication type {iamAuthenticationType} has not been implemented.")
            };

        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }
}
