using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration.ApiGateway.Fixtures;
using AwsSignatureVersion4.Integration.ApiGateway.Requests;
using AwsSignatureVersion4.Private;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway
{
    [Collection("API Gateway")]
    [Trait("Category", "Integration")]
    public class DeleteAsyncShould
    {
        private readonly HttpClient httpClient;
        private readonly string region;
        private readonly string serviceName;
        private readonly string apiGatewayUrl;
        private readonly Func<IamAuthenticationType, AWSCredentials> resolveMutableCredentials;
        private readonly Func<IamAuthenticationType, ImmutableCredentials> resolveImmutableCredentials;

        public DeleteAsyncShould(ApiGatewayFixture fixture)
        {
            httpClient = fixture.HttpClient;
            region = fixture.Region.SystemName;
            serviceName = fixture.ServiceName;
            apiGatewayUrl = fixture.ApiGatewayUrl;
            resolveMutableCredentials = fixture.ResolveMutableCredentials;
            resolveImmutableCredentials = fixture.ResolveImmutableCredentials;
        }

        public static IEnumerable<object[]> TestCases =>
            new[]
            {
                new object[] { IamAuthenticationType.User },
                new object[] { IamAuthenticationType.Role }
            };

        #region DeleteAsync(string, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var response = await httpClient.DeleteAsync(
                apiGatewayUrl,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var response = await httpClient.DeleteAsync(
                apiGatewayUrl,
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region DeleteAsync(Uri, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var response = await httpClient.DeleteAsync(
                apiGatewayUrl.ToUri(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var response = await httpClient.DeleteAsync(
                apiGatewayUrl.ToUri(),
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region DeleteAsync(string, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.DeleteAsync(
                apiGatewayUrl,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.DeleteAsync(
                apiGatewayUrl,
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region DeleteAsync(Uri, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.DeleteAsync(
                apiGatewayUrl.ToUri(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.DeleteAsync(
                apiGatewayUrl.ToUri(),
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task AbortGivenCanceled(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken(true);

            // Act
            var task = httpClient.DeleteAsync(
                apiGatewayUrl,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType),
                ct);

            while (task.Status == TaskStatus.WaitingForActivation)
            {
                await Task.Delay(1);
            }

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }

        #endregion

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenPath(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var path = "/path";

            // Act
            var response = await httpClient.DeleteAsync(
                apiGatewayUrl + path,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe(path);
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenQuery(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(apiGatewayUrl)
            {
                Query = "Param1=Value1"
            };

            // Act
            var response = await httpClient.DeleteAsync(
                uriBuilder.Uri,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value1" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenOrderedQuery(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(apiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=Value2"
            };

            // Act
            var response = await httpClient.DeleteAsync(
                uriBuilder.Uri,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value1", "Value2" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenUnorderedQuery(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(apiGatewayUrl)
            {
                Query = "Param1=Value2&Param1=Value1"
            };

            // Act
            var response = await httpClient.DeleteAsync(
                uriBuilder.Uri,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("DELETE");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value2", "Value1" });
            receivedRequest.Body.ShouldBeNull();
        }
    }
}
