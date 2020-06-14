using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway
{
    public class GetAsyncShould : ApiGatewayIntegrationBase
    {
        public GetAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task Succeed(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenQuery(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=value1"
            };

            // Act
            var response = await HttpClient.GetAsync(
                uriBuilder.Uri,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenOrderedQuery(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=value2"
            };

            // Act
            var response = await HttpClient.GetAsync(
                uriBuilder.Uri,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenUnorderedQuery(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=value2&Param1=Value1"
            };

            // Act
            var response = await HttpClient.GetAsync(
                uriBuilder.Uri,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOption(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenCancellationToken(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public void AbortGivenCanceled(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken(true);

            // Act
            var task = HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOptionAndCancellationToken(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                completionOption,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
