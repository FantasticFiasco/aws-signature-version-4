using System;
using System.Net.Http;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
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
        }

        protected IntegrationTestContext Context { get; }

        protected HttpClient HttpClient { get; }

        public void Dispose() => HttpClient?.Dispose();

        protected ImmutableCredentials ResolveCredentials(IamAuthenticationType iamAuthenticationType)
        {
            switch (iamAuthenticationType)
            {
                case IamAuthenticationType.User: return Context.UserCredentials;
                case IamAuthenticationType.Role: return Context.RoleCredentials;
                default: throw new NotImplementedException($"The authentication type {iamAuthenticationType} is not implemented");
            }
        }
    }
}
