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
        protected IntegrationBase(IntegrationTestContext context)
        {
            Context = context;

            HttpClient = new HttpClient();

            ServiceCollection = new ServiceCollection();
            ServiceCollection
                .AddTransient<AwsSignatureHandler>()
                .AddHttpClient("integration")
                .AddHttpMessageHandler<AwsSignatureHandler>();
        }

        protected IntegrationTestContext Context { get; }

        #region Properties serving tests that use HttpClient

        protected HttpClient HttpClient { get; }

        #endregion

        #region Properties serving tests that use IHttpClientFactory

        protected IServiceCollection ServiceCollection { get; }

        protected IHttpClientFactory HttpClientFactory =>
            ServiceCollection
                .BuildServiceProvider()
                .GetService<IHttpClientFactory>();

        #endregion

        public void Dispose() => HttpClient?.Dispose();

        protected ImmutableCredentials ResolveCredentials(IamAuthenticationType iamAuthenticationType)
        {
            switch (iamAuthenticationType)
            {
                case IamAuthenticationType.User: return Context.UserCredentials.GetCredentials();
                case IamAuthenticationType.Role: return Context.RoleCredentials.GetCredentials();
                default: throw new NotImplementedException($"The authentication type {iamAuthenticationType} is not implemented");
            }
        }
    }
}
