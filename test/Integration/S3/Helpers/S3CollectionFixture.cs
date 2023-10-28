using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.TestSuite;
using Microsoft.Extensions.DependencyInjection;

namespace AwsSignatureVersion4.Integration.S3.Helpers
{
    public class S3CollectionFixture : IDisposable
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IAmazonS3 client;
        private readonly S3Region region;
        private readonly string bucketName;

        public S3CollectionFixture()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection
                .AddTransient<AwsSignatureHandler>()
                .AddHttpClient("integration")
                .AddHttpMessageHandler<AwsSignatureHandler>();

            client = new AmazonS3Client(Secrets.Aws.UserWithProvisioningPermissions.Credentials);
            region = S3Region.FindValue(Secrets.Aws.Region);
            bucketName = $"sigv4-{DateTime.Now:yyyyMMdd-HH.mm.ss.ffff}";

            var request = new PutBucketRequest
            {
                BucketRegion = region,
                BucketName = bucketName
            };

            var response = client.PutBucketAsync(request).Result;
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create S3 bucket, got HTTP status code {response.HttpStatusCode}");
            }

            Bucket = new Bucket(RegionEndpoint.GetBySystemName(RegionName), Secrets.Aws.S3.BucketName, UserCredentials);
        }

        public string RegionName { get; } = Secrets.Aws.Region;

        public string ServiceName { get; } = "s3";

        public AWSCredentials UserCredentials { get; } = Secrets.Aws.UserWithPermissions.Credentials;

        public AWSCredentials RoleCredentials { get; } = new AssumeRoleAWSCredentials(
            Secrets.Aws.UserWithoutPermissions.Credentials,
            Secrets.Aws.Role.Arn,
            "signature-version-4-integration-tests");

        public Bucket Bucket { get; }

        public string S3BucketUrl { get; } = $"https://{Secrets.Aws.S3.BucketName}.s3.{Secrets.Aws.Region}.amazonaws.com";

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

        public IHttpClientFactory HttpClientFactory(IamAuthenticationType iamAuthenticationType) =>
            serviceCollection
                .AddTransient(_ => new AwsSignatureHandlerSettings(
                    RegionName,
                    ServiceName,
                    ResolveMutableCredentials(iamAuthenticationType)))
                .BuildServiceProvider()
                .GetService<IHttpClientFactory>();

        private AWSCredentials ResolveMutableCredentials(
            IamAuthenticationType iamAuthenticationType) =>
            iamAuthenticationType switch
            {
                IamAuthenticationType.User => UserCredentials,
                IamAuthenticationType.Role => RoleCredentials,
                _ => throw new NotImplementedException($"The authentication type {iamAuthenticationType} is not implemented")
            };

    public void Dispose()
        {
            var request = new DeleteBucketRequest
            {
                BucketRegion = region,
                BucketName = bucketName
            };

            var response = client.DeleteBucketAsync(request).Result;
            if (response.HttpStatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception($"Unable to delete S3 bucket, got HTTP status code {response.HttpStatusCode}");
            }
        }
    }
}
