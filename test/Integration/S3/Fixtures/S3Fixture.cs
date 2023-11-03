using System;
using System.Net.Http;
using Amazon;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration.S3.Buckets;

namespace AwsSignatureVersion4.Integration.S3.Fixtures
{
    public class S3Fixture : IDisposable
    {
        private readonly IamCredentials credentials;
        private readonly Services services;

        public S3Fixture()
        {
            credentials = new IamCredentials();
            services = new Services(ResolveMutableCredentials);

            var bucketName = $"sigv4-test-{DateTime.Now:yyyyMMdd-HH.mm.ss.ffff}";
            Bucket = Bucket.CreateAsync(bucketName, Secrets.Aws.UserWithProvisioningPermissions.Credentials, Region).Result;
        }

        public HttpClient HttpClient => new();

        public IHttpClientFactory HttpClientFactory(
            IamAuthenticationType iamAuthenticationType,
            string regionName,
            string serviceName)
            => services.HttpClientFactory(iamAuthenticationType, regionName, serviceName);

        public RegionEndpoint Region => Secrets.Aws.Region;

        public string ServiceName => "s3";

        public AWSCredentials ResolveMutableCredentials(IamAuthenticationType type) => credentials.ResolveMutableCredentials(type);

        public ImmutableCredentials ResolveImmutableCredentials(IamAuthenticationType type) => credentials.ResolveImmutableCredentials(type);

        public Bucket Bucket { get; }

        public void Dispose()
        {
            HttpClient.Dispose();
            Bucket.DeleteAsync().Wait();
        }
    }
}
