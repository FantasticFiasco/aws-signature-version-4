using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Runtime;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite;
using Microsoft.Extensions.DependencyInjection;
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
            var request = BuildRequest(scenarioName);

            ServiceCollection.AddTransient<AwsSignatureHandlerOptions>(_ => new AwsSignatureHandlerOptions(
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(IamAuthenticationType.User)));

            using var httpClient = HttpClientFactory.CreateClient("integration");

            // Act
            var response = await httpClient.SendAsync(request);

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



        //[Fact]
        //public async void Test()
        //{
        //    var services = new ServiceCollection();
        //    services.AddTransient<AwsSignatureHandler>();
        //    services.AddTransient<AwsSignatureHandlerOptions>(provider => new AwsSignatureHandlerOptions("", "", new ImmutableCredentials("a", "b", "c")));

        //    services.AddHttpClient("test",
        //            c =>
        //            {
        //                c.BaseAddress = new Uri("https://www.google.com");
        //                c.DefaultRequestHeaders.Add("SOME-HEADER", "value");
        //            })
        //        .AddHttpMessageHandler<AwsSignatureHandler>();

        //    var client = services
        //        .BuildServiceProvider()
        //        .GetService<IHttpClientFactory>()
        //        .CreateClient("test");

        //    var response = await client.GetStringAsync("/");
        //}
    }
}
