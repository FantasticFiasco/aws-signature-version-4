using System;
using System.Net.Http;
using Amazon;
using Amazon.Runtime;

namespace AwsSignatureVersion4.Integration.ApiGateway.Fixtures
{
    public class ApiGatewayFixture : IDisposable
    {
        private readonly IamCredentials credentials;
        private readonly Services services;

        public ApiGatewayFixture()
        {
            credentials = new IamCredentials();
            services = new Services(ResolveMutableCredentials);
        }

        public HttpClient HttpClient => new();

        public IHttpClientFactory HttpClientFactory(
            IamAuthenticationType iamAuthenticationType,
            string regionName,
            string serviceName)
            => services.HttpClientFactory(iamAuthenticationType, regionName, serviceName);

        public RegionEndpoint Region => Secrets.Aws.Region;

        public string ServiceName => "execute-api";

        public string ApiGatewayUrl => Secrets.Aws.ApiGateway.Url;

        public AWSCredentials ResolveMutableCredentials(IamAuthenticationType type) => credentials.ResolveMutableCredentials(type);

        public ImmutableCredentials ResolveImmutableCredentials(IamAuthenticationType type) => credentials.ResolveImmutableCredentials(type);

        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }
}
