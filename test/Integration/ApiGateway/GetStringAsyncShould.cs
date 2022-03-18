using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.Integration.ApiGateway.Contents;
using AwsSignatureVersion4.Private;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway
{
    public class GetStringAsyncShould : ApiGatewayIntegrationBase
    {
        private static readonly string ExpectedResponseContent = new RichContent().ToJson();

        public GetStringAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        #region GetStringAsync(string, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenRequestStringAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var stringContent =  await HttpClient.GetStringAsync(
                Context.ApiGatewayUrl,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenRequestStringAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var stringContent =  await HttpClient.GetStringAsync(
                Context.ApiGatewayUrl,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
        }

        #endregion

        #region GetStringAsync(Uri, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenRequestUriAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var stringContent =  await HttpClient.GetStringAsync(
                Context.ApiGatewayUrl.ToUri(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenRequestUriAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var stringContent =  await HttpClient.GetStringAsync(
                Context.ApiGatewayUrl.ToUri(),
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
        }

        #endregion

        #region GetStringAsync(string, CancellationToken, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var stringContent =  await HttpClient.GetStringAsync(
                Context.ApiGatewayUrl,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var stringContent =  await HttpClient.GetStringAsync(
                Context.ApiGatewayUrl,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task AbortGivenCanceled(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken(true);

            // Act
            var task = HttpClient.GetStringAsync(
                Context.ApiGatewayUrl,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            while (task.Status == TaskStatus.WaitingForActivation)
            {
                await Task.Delay(1);
            }

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }

        #endregion

        #region GetStringAsync(Uri, CancellationToken, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var stringContent =  await HttpClient.GetStringAsync(
                Context.ApiGatewayUrl.ToUri(),
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var stringContent =  await HttpClient.GetStringAsync(
                Context.ApiGatewayUrl.ToUri(),
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
        }

        #endregion

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
            var stringContent =  await HttpClient.GetStringAsync(
                uriBuilder.Uri,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
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
            var stringContent =  await HttpClient.GetStringAsync(
                uriBuilder.Uri,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
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
            var stringContent =  await HttpClient.GetStringAsync(
                uriBuilder.Uri,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            stringContent.ShouldBe(ExpectedResponseContent);
        }
    }
}
