using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration.ApiGateway.Contents;
using AwsSignatureVersion4.Integration.ApiGateway.Fixtures;
using AwsSignatureVersion4.Integration.ApiGateway.Requests;
using AwsSignatureVersion4.Private;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway
{
    [Collection("API Gateway")]
    [Trait("Category", "Integration")]
    public class PostAsyncShould
    {
        private readonly HttpClient httpClient;
        private readonly string region;
        private readonly string serviceName;
        private readonly string apiGatewayUrl;
        private readonly Func<IamAuthenticationType, AWSCredentials> resolveMutableCredentials;
        private readonly Func<IamAuthenticationType, ImmutableCredentials> resolveImmutableCredentials;

        public PostAsyncShould(ApiGatewayFixture fixture)
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
                new object[] { IamAuthenticationType.User, new EmptyJsonContent() },
                new object[] { IamAuthenticationType.User, new RichJsonContent() },
                new object[] { IamAuthenticationType.User, new BinaryContent() },
                new object[] { IamAuthenticationType.Role, new EmptyJsonContent() },
                new object[] { IamAuthenticationType.Role, new RichJsonContent() },
                new object[] { IamAuthenticationType.Role, new BinaryContent() }
            };

        #region PostAsync(string, HttpContent, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndMutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Act
            var response = await httpClient.PostAsync(
                apiGatewayUrl,
                content.AsHttpContent(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndImmutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Act
            var response = await httpClient.PostAsync(
                apiGatewayUrl,
                content.AsHttpContent(),
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        #endregion

        #region PostAsync(Uri, HttpContent, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndMutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Act
            var response = await httpClient.PostAsync(
                apiGatewayUrl.ToUri(),
                content.AsHttpContent(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndImmutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Act
            var response = await httpClient.PostAsync(
                apiGatewayUrl.ToUri(),
                content.AsHttpContent(),
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        #endregion

        #region PostAsync(string, HttpContent, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndMutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.PostAsync(
                apiGatewayUrl,
                content.AsHttpContent(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndImmutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.PostAsync(
                apiGatewayUrl,
                content.AsHttpContent(),
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task AbortGivenCanceled(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Arrange
            var ct = new CancellationToken(true);

            // Act
            var task = httpClient.PostAsync(
                apiGatewayUrl,
                content.AsHttpContent(),
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

        #region PostAsync(Uri, HttpContent, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndMutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.PostAsync(
                apiGatewayUrl.ToUri(),
                content.AsHttpContent(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndImmutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.PostAsync(
                apiGatewayUrl.ToUri(),
                content.AsHttpContent(),
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        #endregion

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenPath(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Arrange
            var path = "/path";

            // Act
            var response = await httpClient.PostAsync(
                apiGatewayUrl + path,
                content.AsHttpContent(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe(path);
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenQuery(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Arrange
            var uriBuilder = new UriBuilder(apiGatewayUrl)
            {
                Query = "Param1=Value1"
            };

            // Act
            var response = await httpClient.PostAsync(
                uriBuilder.Uri,
                content.AsHttpContent(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value1" });
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenOrderedQuery(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Arrange
            var uriBuilder = new UriBuilder(apiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=Value2"
            };

            // Act
            var response = await httpClient.PostAsync(
                uriBuilder.Uri,
                content.AsHttpContent(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value1", "Value2" });
            receivedRequest.Body.ShouldBe(content.AsString());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenUnorderedQuery(
            IamAuthenticationType iamAuthenticationType,
            IContent content)
        {
            // Arrange
            var uriBuilder = new UriBuilder(apiGatewayUrl)
            {
                Query = "Param1=Value2&Param1=Value1"
            };

            // Act
            var response = await httpClient.PostAsync(
                uriBuilder.Uri,
                content.AsHttpContent(),
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("POST");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value2", "Value1" });
            receivedRequest.Body.ShouldBe(content.AsString());
        }
    }
}
