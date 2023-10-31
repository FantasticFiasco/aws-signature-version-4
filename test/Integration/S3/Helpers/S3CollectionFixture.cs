using System;
using System.IO;
using System.Net.Http;
using Amazon;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.TestSuite;
using Microsoft.Extensions.DependencyInjection;

namespace AwsSignatureVersion4.Integration.S3.Helpers
{
    public class S3CollectionFixture : IDisposable
    {
        private readonly IServiceCollection serviceCollection;
        
        public S3CollectionFixture()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection
                .AddTransient<AwsSignatureHandler>()
                .AddHttpClient("integration")
                .AddHttpMessageHandler<AwsSignatureHandler>();

            var bucketName = $"sigv4-test-{DateTime.Now:yyyyMMdd-HH.mm.ss.ffff}";
            Bucket = Bucket.CreateAsync(bucketName, Secrets.Aws.UserWithProvisioningPermissions.Credentials, Secrets.Aws.Region).Result;

            HttpClient = new HttpClient();
        }

        public RegionEndpoint Region { get; } = Secrets.Aws.Region;

        public string ServiceName { get; } = "s3";

        public AWSCredentials UserCredentials { get; } = Secrets.Aws.UserWithPermissions.Credentials;

        public AWSCredentials RoleCredentials { get; } = new AssumeRoleAWSCredentials(
            Secrets.Aws.UserWithoutPermissions.Credentials,
            Secrets.Aws.Role.Arn,
            "signature-version-4-integration-tests");

        public Bucket Bucket { get; }

        

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

        public HttpClient HttpClient { get; }

        public IHttpClientFactory HttpClientFactory(IamAuthenticationType iamAuthenticationType) =>
            serviceCollection
                .AddTransient(
                    _ => new AwsSignatureHandlerSettings(
                        Secrets.Aws.Region.SystemName,
                        ServiceName,
                        ResolveCredentials(iamAuthenticationType)))
                .BuildServiceProvider()
                .GetService<IHttpClientFactory>();

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
            Bucket.DeleteAsync().Wait();
            HttpClient.Dispose();
        }
    }
}
