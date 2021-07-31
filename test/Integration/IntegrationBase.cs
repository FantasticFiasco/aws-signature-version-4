using System;
using System.Net.Http;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AwsSignatureVersion4.Integration
{
    [Trait("Category", "Integration")]
    public abstract class IntegrationBase : IClassFixture<IntegrationTestContext>, IDisposable
    {
        private readonly IServiceCollection serviceCollection;

        protected IntegrationBase(IntegrationTestContext context)
        {
            Context = context;

            HttpClient = new HttpClient();

            serviceCollection = new ServiceCollection();
            serviceCollection
                .AddTransient<AwsSignatureHandler>()
                .AddHttpClient("integration")
                .AddHttpMessageHandler<AwsSignatureHandler>();
        }

        protected IntegrationTestContext Context { get; }

        #region Members serving tests that use HttpClient

        protected HttpClient HttpClient { get; }

        #endregion

        #region Members serving tests that use IHttpClientFactory

        protected IHttpClientFactory HttpClientFactory(IamAuthenticationType iamAuthenticationType) =>
            serviceCollection
                .AddTransient(_ => new AwsSignatureHandlerSettings(
                    Context.RegionName,
                    Context.ServiceName,
                    ResolveMutableCredentials(iamAuthenticationType)))
                .BuildServiceProvider()
                .GetService<IHttpClientFactory>();

        #endregion

        public void Dispose() => HttpClient?.Dispose();

        protected AWSCredentials ResolveMutableCredentials(IamAuthenticationType iamAuthenticationType) =>
            iamAuthenticationType switch
            {
                IamAuthenticationType.User => Context.UserCredentials,
                IamAuthenticationType.Role => Context.RoleCredentials,
                _ => throw new NotImplementedException($"The authentication type {iamAuthenticationType} is not implemented")
            };

        protected ImmutableCredentials ResolveImmutableCredentials(IamAuthenticationType iamAuthenticationType) =>
            ResolveMutableCredentials(iamAuthenticationType).GetCredentials();
    }
}
