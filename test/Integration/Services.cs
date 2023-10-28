using System;
using System.Net.Http;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AwsSignatureVersion4.Integration
{
    public class Services
    {
        private readonly IServiceCollection serviceCollection;

        public Services()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection
                .AddTransient<AwsSignatureHandler>()
                .AddHttpClient("integration")
                .AddHttpMessageHandler<AwsSignatureHandler>();
        }

        public IHttpClientFactory HttpClientFactory(
            IamAuthenticationType iamAuthenticationType,
            AWSCredentials userCredentials,
            AWSCredentials roleCredentials,
            string regionName,
            string serviceName) =>
            serviceCollection
                .AddTransient(_ => new AwsSignatureHandlerSettings(
                    regionName,
                    serviceName,
                    ResolveMutableCredentials(iamAuthenticationType, userCredentials, roleCredentials)))
                .BuildServiceProvider()
                .GetService<IHttpClientFactory>();

        private AWSCredentials ResolveMutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            AWSCredentials userCredentials,
            AWSCredentials roleCredentials) =>
            iamAuthenticationType switch
            {
                IamAuthenticationType.User => userCredentials,
                IamAuthenticationType.Role => roleCredentials,
                _ => throw new NotImplementedException($"The authentication type {iamAuthenticationType} is not implemented")
            };
    }
}
