using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway
{
    public class SendShould : ApiGatewayIntegrationBase, IClassFixture<TestSuiteContext>
    {
        private readonly TestSuiteContext testSuiteContext;

        public SendShould(IntegrationTestContext context, TestSuiteContext testSuiteContext)
            : base(context)
        {
            this.testSuiteContext = testSuiteContext;
        }

        #region Send(HttpRequestMessage, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenMutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenImmutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        #endregion

        #region Send(HttpRequestMessage, HttpCompletionOption, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public void SucceedGivenHttpCompletionOptionAndMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, Context.ApiGatewayUrl);
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = HttpClient.Send(
                request,
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public void SucceedGivenHttpCompletionOptionAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, Context.ApiGatewayUrl);
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = HttpClient.Send(
                request,
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        #endregion

        #region Send(HttpRequestMessage, CancellationToken, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenCancellationTokenAndMutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            var ct = new CancellationToken();

            // Act
            var response = HttpClient.Send(
                request,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenCancellationTokenAndImmutableCredentials(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            var ct = new CancellationToken();

            // Act
            var response = HttpClient.Send(
                request,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        #endregion

        #region Send(HttpRequestMessage, HttpCompletionOption, CancellationToken, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public void SucceedGivenHttpCompletionOptionAndCancellationTokenMutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, Context.ApiGatewayUrl);
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = HttpClient.Send(
                request,
                completionOption,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public void SucceedGivenHttpCompletionOptionAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, Context.ApiGatewayUrl);
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = HttpClient.Send(
                request,
                completionOption,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
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
        public void PassTestSuiteGivenUserWithPermissions(params string[] scenarioName)
        {
            // Arrange
            var request = BuildRequest(scenarioName);
            var iamAuthenticationType = IamAuthenticationType.User;

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

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
        public void PassTestSuiteGivenAssumedRole(params string[] scenarioName)
        {
            // Arrange
            var request = BuildRequest(scenarioName);
            var iamAuthenticationType = IamAuthenticationType.Role;

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenHeaderWithDuplicateValues(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value2", "value2" });

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenHeaderWithUnorderedValues(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value4", "value1", "value3", "value2" });

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenHeaderWithWhitespaceCharacters(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value1", "a   b   c" });

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenQuery(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=value1"
            };

            var request = new HttpRequestMessage(new HttpMethod(method), uriBuilder.Uri);

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenOrderedQuery(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=value2"
            };

            var request = new HttpRequestMessage(new HttpMethod(method), uriBuilder.Uri);

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public void SucceedGivenUnorderedQuery(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=value2&Param1=Value1"
            };

            var request = new HttpRequestMessage(new HttpMethod(method), uriBuilder.Uri);

            // Act
            var response = HttpClient.Send(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private HttpRequestMessage BuildRequest(string[] scenarioName)
        {
            var request = testSuiteContext.LoadScenario(scenarioName).Request;

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