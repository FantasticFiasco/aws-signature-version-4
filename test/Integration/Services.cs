using System;
using System.Net.Http;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace AwsSignatureVersion4.Integration
{
    public class Services
    {
        private readonly Func<IamAuthenticationType, AWSCredentials> resolveMutableCredentials;
        private readonly Func<IServiceCollection> serviceCollectionProvider;
        
        public Services(Func<IamAuthenticationType, AWSCredentials> resolveMutableCredentials)
        {
            this.resolveMutableCredentials = resolveMutableCredentials;

            serviceCollectionProvider = () =>
            {
                var serviceCollection = new ServiceCollection();

                serviceCollection
                    .AddTransient<AwsSignatureHandler>()
                    .AddHttpClient("integration")
                    .AddHttpMessageHandler<AwsSignatureHandler>();

                return serviceCollection;
            };
        }

        public IHttpClientFactory HttpClientFactory(
            IamAuthenticationType iamAuthenticationType,
            string regionName,
            string serviceName)
        {
            var serviceProvider = serviceCollectionProvider()
                .AddTransient(
                    _ => new AwsSignatureHandlerSettings(
                        regionName,
                        serviceName,
                        resolveMutableCredentials(iamAuthenticationType)))
                .BuildServiceProvider();


            return serviceProvider.GetService<IHttpClientFactory>();
        }
    }
}
