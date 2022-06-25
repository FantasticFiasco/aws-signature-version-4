using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.Integration.ApiGateway.Requests;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway
{
    public class AwsSignatureHandlerShould : ApiGatewayIntegrationBase, IClassFixture<TestSuiteContext>
    {
        private readonly TestSuiteContext testSuiteContext;

        public AwsSignatureHandlerShould(IntegrationTestContext context, TestSuiteContext testSuiteContext)
            : base(context)
        {
            this.testSuiteContext = testSuiteContext;
        }

        public static IEnumerable<object[]> TestCases =>
            new[]
            {
                new object[] { IamAuthenticationType.User, HttpMethod.Get },
                new object[] { IamAuthenticationType.User, HttpMethod.Post },
                new object[] { IamAuthenticationType.User, HttpMethod.Put },
                new object[] { IamAuthenticationType.User, HttpMethod.Patch },
                new object[] { IamAuthenticationType.User, HttpMethod.Delete },
                new object[] { IamAuthenticationType.Role, HttpMethod.Get },
                new object[] { IamAuthenticationType.Role, HttpMethod.Post },
                new object[] { IamAuthenticationType.Role, HttpMethod.Put },
                new object[] { IamAuthenticationType.Role, HttpMethod.Patch },
                new object[] { IamAuthenticationType.Role, HttpMethod.Delete }
            };

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
            using var httpClient = HttpClientFactory(IamAuthenticationType.User).CreateClient("integration");
            var request = BuildRequest(scenarioName);

            // Act
            var response = await httpClient.SendAsync(request);

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
            using var httpClient = HttpClientFactory(IamAuthenticationType.Role).CreateClient("integration");
            var request = BuildRequest(scenarioName);

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenPath(
            IamAuthenticationType iamAuthenticationType,
            HttpMethod method)
        {
            // Arrange
            using var httpClient = HttpClientFactory(iamAuthenticationType).CreateClient("integration");
            var path = "/path";
            var request = new HttpRequestMessage(method, Context.ApiGatewayUrl + path);

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method.ToString());
            receivedRequest.Path.ShouldBe(path);
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenHeaderWithDuplicateValues(
            IamAuthenticationType iamAuthenticationType,
            HttpMethod method)
        {
            // Arrange
            using var httpClient = HttpClientFactory(iamAuthenticationType).CreateClient("integration");
            var request = new HttpRequestMessage(method, Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value2", "value2" });

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method.ToString());
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Headers["My-Header1"].ShouldBe(new[] { "value2, value2" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenHeaderWithUnorderedValues(
            IamAuthenticationType iamAuthenticationType,
            HttpMethod method)
        {
            // Arrange
            using var httpClient = HttpClientFactory(iamAuthenticationType).CreateClient("integration");
            var request = new HttpRequestMessage(method, Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value4", "value1", "value3", "value2" });

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method.ToString());
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Headers["My-Header1"].ShouldBe(new[] { "value4, value1, value3, value2" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenHeaderWithWhitespaceCharacters(
            IamAuthenticationType iamAuthenticationType,
            HttpMethod method)
        {
            // Arrange
            using var httpClient = HttpClientFactory(iamAuthenticationType).CreateClient("integration");
            var request = new HttpRequestMessage(method, Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value1", "a   b   c" });

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method.ToString());
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Headers["My-Header1"].ShouldBe(new[] { "value1, a   b   c" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenQuery(
            IamAuthenticationType iamAuthenticationType,
            HttpMethod method)
        {
            // Arrange
            using var httpClient = HttpClientFactory(iamAuthenticationType).CreateClient("integration");

            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1"
            };

            var request = new HttpRequestMessage(method, uriBuilder.Uri);

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method.ToString());
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value1" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenOrderedQuery(
            IamAuthenticationType iamAuthenticationType,
            HttpMethod method)
        {
            // Arrange
            using var httpClient = HttpClientFactory(iamAuthenticationType).CreateClient("integration");

            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=Value2"
            };

            var request = new HttpRequestMessage(method, uriBuilder.Uri);

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method.ToString());
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value1", "Value2" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenUnorderedQuery(
            IamAuthenticationType iamAuthenticationType,
            HttpMethod method)
        {
            // Arrange
            using var httpClient = HttpClientFactory(iamAuthenticationType).CreateClient("integration");

            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value2&Param1=Value1"
            };

            var request = new HttpRequestMessage(method, uriBuilder.Uri);

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method.ToString());
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters["Param1"].ShouldBe(new[] { "Value2", "Value1" });
            receivedRequest.Body.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task SucceedGivenHttpCompletionOption(
            IamAuthenticationType iamAuthenticationType,
            HttpMethod method)
        {
            // Arrange
            using var httpClient = HttpClientFactory(iamAuthenticationType).CreateClient("integration");
            var request = new HttpRequestMessage(method, Context.ApiGatewayUrl);
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await httpClient.SendAsync(request, completionOption);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe(method.ToString());
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryStringParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBeNull();
        }

        private HttpRequestMessage BuildRequest(string[] scenarioName)
        {
            var request = testSuiteContext.LoadScenario(scenarioName).Request;

            if (request.RequestUri == null) throw new Exception("Test suite request URI cannot be null");

            // Redirect the request to the AWS API Gateway
            request.RequestUri = request.RequestUri
                .ToString()
                .Replace("https://example.amazonaws.com", Context.ApiGatewayUrl)
                .ToUri();

            // The "Host" header is now invalid since we redirected the request to the AWS API
            // Gateway. Lets remove the header and have the signature implementation re-add it
            // correctly.
            request.Headers.Remove("Host");

            return request;
        }
    }
}
