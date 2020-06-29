using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.Integration.ApiGateway.Contents;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway
{
    public class PostAsyncShould : ApiGatewayIntegrationBase
    {
        public PostAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
        public async Task Succeed(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Act
            var response = await HttpClient.PostAsync(
                Context.ApiGatewayUrl,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
        public async Task SucceedGivenQuery(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=value1"
            };

            // Act
            var response = await HttpClient.PostAsync(
                uriBuilder.Uri,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
        public async Task SucceedGivenOrderedQuery(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=value2"
            };

            // Act
            var response = await HttpClient.PostAsync(
                uriBuilder.Uri,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
        public async Task SucceedGivenUnorderedQuery(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=value2&Param1=Value1"
            };

            // Act
            var response = await HttpClient.PostAsync(
                uriBuilder.Uri,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
        public async Task SucceedGivenCancellationToken(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.PostAsync(
                Context.ApiGatewayUrl,
                contentType.ToJsonContent(),
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
        public void AbortGivenCanceled(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var ct = new CancellationToken(true);

            // Act
            var task = HttpClient.PostAsync(
                Context.ApiGatewayUrl,
                contentType.ToJsonContent(),
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }
    }
}
