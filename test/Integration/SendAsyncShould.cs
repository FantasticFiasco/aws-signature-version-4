using System.Threading.Tasks;
using AWS.SignatureVersion4;
using AWS.SignatureVersion4.Private;
using AWS.SignatureVersion4.Integration;
using AWS.SignatureVersion4.Integration.Authentication;
using AWS.SignatureVersion4.TestSuite;
using Shouldly;
using Xunit;

// ReSharper disable once CheckNamespace
namespace System.Net.Http
{
    public class SendAsyncShould : IntegrationBase, IClassFixture<TestSuiteContext>
    {
        private readonly TestSuiteContext testSuiteContext;

        public SendAsyncShould(IntegrationTestContext context, TestSuiteContext testSuiteContext)
            : base(context)
        {
            this.testSuiteContext = testSuiteContext;
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
        [InlineData("get-vanilla-query-unreserved", Skip = SkipReasons.UnsupportedByApiGateway)]
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
        [InlineData("post-x-www-form-urlencoded", Skip = SkipReasons.PlausibleSignedHeadersTestSuiteError)]
        [InlineData("post-x-www-form-urlencoded-parameters", Skip = SkipReasons.RedundantContentTypeCharset)]
        public async Task PassTestSuiteGivenUserWithPermissions(params string[] scenarioName)
        {
            // Arrange
            var request = BuildRequest(scenarioName);
            var authenticationType = IamAuthenticationType.User;

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(authenticationType));

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
        [InlineData("get-vanilla-query-unreserved", Skip = SkipReasons.UnsupportedByApiGateway)]
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
        [InlineData("post-x-www-form-urlencoded", Skip = SkipReasons.PlausibleSignedHeadersTestSuiteError)]
        [InlineData("post-x-www-form-urlencoded-parameters", Skip = SkipReasons.RedundantContentTypeCharset)]
        public async Task PassTestSuiteGivenAssumedRole(params string[] scenarioName)
        {
            // Arrange
            var request = BuildRequest(scenarioName);
            var authenticationType = IamAuthenticationType.Role;

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(authenticationType));

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
        public async Task SucceedGivenHeaderWithDuplicateValues(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value2", "value2" });

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

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
        public async Task SucceedGivenHeaderWithUnorderedValues(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value4", "value1", "value3", "value2" });

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

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
        public async Task SucceedGivenHeaderWithWhitespaceCharacters(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value1", "a   b   c" });

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

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
        public async Task SucceedGivenQuery(
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
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

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
        public async Task SucceedGivenOrderedQuery(
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
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

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
        public async Task SucceedGivenUnorderedQuery(
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
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private HttpRequestMessage BuildRequest(string[] scenarioName)
        {
            var request = testSuiteContext.LoadScenario(scenarioName).Request;

            // Redirect the request to the API Gateway
            request.RequestUri = request.RequestUri
                .ToString()
                .Replace("https://example.amazonaws.com", Context.ApiGatewayUrl.ToString())
                .ToUri();

            // The "Host" header is now invalid since we redirected the request to the API Gateway.
            // Lets remove the header and have the signature implementation re-add it correctly.
            request.Headers.Remove("Host");

            return request;
        }
    }
}
