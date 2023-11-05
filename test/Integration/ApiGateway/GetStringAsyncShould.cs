using System;
using System.Collections.Generic;
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
    public class GetStringAsyncShould
    {
        private readonly HttpClient httpClient;
        private readonly string region;
        private readonly string serviceName;
        private readonly string apiGatewayUrl;
        private readonly Func<IamAuthenticationType, AWSCredentials> resolveMutableCredentials;
        private readonly Func<IamAuthenticationType, ImmutableCredentials> resolveImmutableCredentials;

        public GetStringAsyncShould(ApiGatewayFixture fixture)
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

        #region GetStringAsync(string, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var stringContent = await httpClient.GetStringAsync(
                apiGatewayUrl,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var stringContent = await httpClient.GetStringAsync(
                apiGatewayUrl,
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType));

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region GetStringAsync(Uri, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var stringContent = await httpClient.GetStringAsync(
                apiGatewayUrl.ToUri(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var stringContent = await httpClient.GetStringAsync(
                apiGatewayUrl.ToUri(),
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType));

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region GetStringAsync(string, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var stringContent = await httpClient.GetStringAsync(
                apiGatewayUrl,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
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
            var stringContent = await httpClient.GetStringAsync(
                apiGatewayUrl,
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
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
            var task = httpClient.GetStringAsync(
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

        #region GetStringAsync(Uri, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var stringContent = await httpClient.GetStringAsync(
                apiGatewayUrl.ToUri(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
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
            var stringContent = await httpClient.GetStringAsync(
                apiGatewayUrl.ToUri(),
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenPath(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var path = "/path";

            // Act
            var stringContent = await httpClient.GetStringAsync(
                apiGatewayUrl + path,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
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
            var stringContent = await httpClient.GetStringAsync(
                uriBuilder.Uri,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
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
            var stringContent = await httpClient.GetStringAsync(
                uriBuilder.Uri,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
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
            var stringContent = await httpClient.GetStringAsync(
                uriBuilder.Uri,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            var receivedRequest = stringContent.DeserializeReceivedRequest();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value2", "Value1" });
            receivedRequest.Body.ShouldBeNull();
        }
    }
}
