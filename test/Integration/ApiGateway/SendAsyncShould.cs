using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration.ApiGateway.Fixtures;
using AwsSignatureVersion4.Integration.ApiGateway.Requests;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite;
using AwsSignatureVersion4.TestSuite.Fixtures;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway
{
    [Collection("API Gateway")]
    [Trait("Category", "Integration")]
    public class SendAsyncShould
    {
        private readonly HttpClient httpClient;
        private readonly string region;
        private readonly string serviceName;
        private readonly string apiGatewayUrl;
        private readonly Func<IamAuthenticationType, AWSCredentials> resolveMutableCredentials;
        private readonly Func<IamAuthenticationType, ImmutableCredentials> resolveImmutableCredentials;

        private readonly Func<string[], Scenario> loadScenario;
        private readonly Func<HttpRequestMessage, string, HttpRequestMessage> redirectRequest;

        public SendAsyncShould(ApiGatewayCollectionFixture apiGatewayCollectionFixture, TestSuiteFixture testSuiteFixture)
        {
            httpClient = apiGatewayCollectionFixture.HttpClient;
            region = apiGatewayCollectionFixture.Region.SystemName;
            serviceName = apiGatewayCollectionFixture.ServiceName;
            apiGatewayUrl = apiGatewayCollectionFixture.ApiGatewayUrl;
            resolveMutableCredentials = apiGatewayCollectionFixture.ResolveMutableCredentials;
            resolveImmutableCredentials = apiGatewayCollectionFixture.ResolveImmutableCredentials;

            loadScenario = testSuiteFixture.LoadScenario;
            redirectRequest = testSuiteFixture.RedirectRequest;
        }

        #region SendAsync(HttpRequestMessage, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenMutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), apiGatewayUrl);

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenImmutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), apiGatewayUrl);

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region SendAsync(HttpRequestMessage, HttpCompletionOption, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOptionAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, apiGatewayUrl);
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await httpClient.SendAsync(
                request,
                completionOption,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOptionAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, apiGatewayUrl);
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await httpClient.SendAsync(
                request,
                completionOption,
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("GET");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region SendAsync(HttpRequestMessage, CancellationToken, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenCancellationTokenAndMutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), apiGatewayUrl);
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenCancellationTokenAndImmutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), apiGatewayUrl);
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        #endregion

        #region SendAsync(HttpRequestMessage, HttpCompletionOption, CancellationToken, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOptionAndCancellationTokenMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, apiGatewayUrl);
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.SendAsync(
                request,
                completionOption,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType),
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
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOptionAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, apiGatewayUrl);
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = await httpClient.SendAsync(
                request,
                completionOption,
                region,
                serviceName,
                resolveImmutableCredentials(iamAuthenticationType),
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
        [InlineData("get-header-key-duplicate")]
        [InlineData("get-header-value-multiline")]
        [InlineData("get-header-value-order")]
        [InlineData("get-header-value-trim")]
        [InlineData("get-unreserved")]
        [InlineData("get-utf8")]
        [InlineData("get-vanilla")]
        [InlineData("get-vanilla-empty-query-key")]
        [InlineData("get-vanilla-query")]
        [InlineData("get-vanilla-query-order-key")]
        [InlineData("get-vanilla-query-order-key-case")]
        [InlineData("get-vanilla-query-order-value")]
        [InlineData("get-vanilla-query-unreserved", Skip = SkipReasons.NotSupportedByApiGateway)]
        [InlineData("get-vanilla-utf8-query")]
        [InlineData("normalize-path", "get-relative")]
        [InlineData("normalize-path", "get-relative-relative")]
        [InlineData("normalize-path", "get-slash")]
        [InlineData("normalize-path", "get-slash-dot-slash")]
        [InlineData("normalize-path", "get-slashes")]
        [InlineData("normalize-path", "get-slash-pointless-dot")]
        [InlineData("normalize-path", "get-space")]
        [InlineData("post-header-key-case")]
        [InlineData("post-header-key-sort")]
        [InlineData("post-header-value-case")]
        [InlineData("post-sts-token", "post-sts-header-after")]
        [InlineData("post-sts-token", "post-sts-header-before", Skip = SkipReasons.RedundantStsTokenScenario)]
        [InlineData("post-vanilla")]
        [InlineData("post-vanilla-empty-query-value")]
        [InlineData("post-vanilla-query")]
        [InlineData("post-x-www-form-urlencoded")]
        [InlineData("post-x-www-form-urlencoded-parameters", Skip = SkipReasons.RedundantContentTypeCharset)]
        public async Task PassTestSuiteGivenUserWithPermissions(params string[] scenarioName)
        {
            // Arrange
            var scenario = loadScenario(scenarioName);
            var request = redirectRequest(scenario.Request, apiGatewayUrl);
            var iamAuthenticationType = IamAuthenticationType.User;

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("get-header-key-duplicate")]
        [InlineData("get-header-value-multiline")]
        [InlineData("get-header-value-order")]
        [InlineData("get-header-value-trim")]
        [InlineData("get-unreserved")]
        [InlineData("get-utf8")]
        [InlineData("get-vanilla")]
        [InlineData("get-vanilla-empty-query-key")]
        [InlineData("get-vanilla-query")]
        [InlineData("get-vanilla-query-order-key")]
        [InlineData("get-vanilla-query-order-key-case")]
        [InlineData("get-vanilla-query-order-value")]
        [InlineData("get-vanilla-query-unreserved", Skip = SkipReasons.NotSupportedByApiGateway)]
        [InlineData("get-vanilla-utf8-query")]
        [InlineData("normalize-path", "get-relative")]
        [InlineData("normalize-path", "get-relative-relative")]
        [InlineData("normalize-path", "get-slash")]
        [InlineData("normalize-path", "get-slash-dot-slash")]
        [InlineData("normalize-path", "get-slashes")]
        [InlineData("normalize-path", "get-slash-pointless-dot")]
        [InlineData("normalize-path", "get-space")]
        [InlineData("post-header-key-case")]
        [InlineData("post-header-key-sort")]
        [InlineData("post-header-value-case")]
        [InlineData("post-sts-token", "post-sts-header-after")]
        [InlineData("post-sts-token", "post-sts-header-before", Skip = SkipReasons.RedundantStsTokenScenario)]
        [InlineData("post-vanilla")]
        [InlineData("post-vanilla-empty-query-value")]
        [InlineData("post-vanilla-query")]
        [InlineData("post-x-www-form-urlencoded")]
        [InlineData("post-x-www-form-urlencoded-parameters", Skip = SkipReasons.RedundantContentTypeCharset)]
        public async Task PassTestSuiteGivenAssumedRole(params string[] scenarioName)
        {
            // Arrange
            var scenario = loadScenario(scenarioName);
            var request = redirectRequest(scenario.Request, apiGatewayUrl);
            var iamAuthenticationType = IamAuthenticationType.Role;

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenPath(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var path = "/path";
            var request = new HttpRequestMessage(new HttpMethod(method), apiGatewayUrl + path);

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe(path);
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenHeaderWithDuplicateValues(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), apiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value2", "value2" });

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Headers["My-Header1"].ShouldBe(new[] { "value2, value2" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenHeaderWithUnorderedValues(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), apiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value4", "value1", "value3", "value2" });

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Headers["My-Header1"].ShouldBe(new[] { "value4, value1, value3, value2" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenHeaderWithWhitespaceCharacters(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), apiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value1", "a   b   c" });

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Headers["My-Header1"].ShouldBe(new[] { "value1, a   b   c" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenQuery(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var uriBuilder = new UriBuilder(apiGatewayUrl)
            {
                Query = "Param1=Value1"
            };

            var request = new HttpRequestMessage(new HttpMethod(method), uriBuilder.Uri);

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value1" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenOrderedQuery(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var uriBuilder = new UriBuilder(apiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=Value2"
            };

            var request = new HttpRequestMessage(new HttpMethod(method), uriBuilder.Uri);

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value1", "Value2" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "PATCH")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "PATCH")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenUnorderedQuery(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var uriBuilder = new UriBuilder(apiGatewayUrl)
            {
                Query = "Param1=Value2&Param1=Value1"
            };

            var request = new HttpRequestMessage(new HttpMethod(method), uriBuilder.Uri);

            // Act
            var response = await httpClient.SendAsync(
                request,
                region,
                serviceName,
                resolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method);
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value2", "Value1" });
            receivedRequest.Body.ShouldBeNull();
        }
    }
}
