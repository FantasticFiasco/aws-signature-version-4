using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.Integration.ApiGateway.Requests;
using AwsSignatureVersion4.Private;
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

        public static IEnumerable<object[]> TestCases =>
            new[]
            {
                new object[] { IamAuthenticationType.User },
                new object[] { IamAuthenticationType.Role }
            };

        #region GetAsync(string, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
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
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region GetAsync(Uri, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl.ToUri(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
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
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl.ToUri(),
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region GetAsync(string, HttpCompletionOption, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndHttpCompletionOptionAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndHttpCompletionOptionAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region GetAsync(Uri, HttpCompletionOption, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndHttpCompletionOptionAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl.ToUri(),
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndHttpCompletionOptionAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl.ToUri(),
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region GetAsync(string, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
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
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
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
            var task = HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            while (task.Status == TaskStatus.WaitingForActivation)
            {
                await Task.Delay(1);
            }

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }

        #endregion

        #region GetAsync(Uri, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl.ToUri(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
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
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl.ToUri(),
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region GetAsync(string, HttpCompletionOption, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndHttpCompletionOptionAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestStringAndHttpCompletionOptionAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region GetAsync(Uri, HttpCompletionOption, CancellationToken, string, string, <credentials>)

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndHttpCompletionOptionAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl.ToUri(),
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenRequestUriAndHttpCompletionOptionAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl.ToUri(),
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
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
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl + path,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
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
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1"
            };

            // Act
            var response = await HttpClient.GetAsync(
                uriBuilder.Uri,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
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
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=Value2"
            };

            // Act
            var response = await HttpClient.GetAsync(
                uriBuilder.Uri,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
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
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value2&Param1=Value1"
            };

            // Act
            var response = await HttpClient.GetAsync(
                uriBuilder.Uri,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value2", "Value1" });
            receivedRequest.Body.ShouldBeNull();
        }
    }
}
