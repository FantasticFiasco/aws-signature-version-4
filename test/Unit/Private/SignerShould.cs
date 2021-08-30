using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Amazon.Util;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit.Private
{
    public class SignerShould : IClassFixture<TestSuiteContext>, IDisposable
    {
        private readonly TestSuiteContext context;
        private readonly HttpClient httpClient;

        public SignerShould(TestSuiteContext context)
        {
            this.context = context;

            httpClient = new HttpClient();

            context.AdjustHeaderValueSeparator();
        }

        [Theory]
        [InlineData("get-header-key-duplicate")]
        [InlineData("get-header-value-multiline")]
        [InlineData("get-header-value-order")]
        [InlineData("get-header-value-trim")]
        [InlineData("get-unreserved")]
        [InlineData("get-utf8", Skip = SkipReasons.PlausibleCanonicalUriTestSuiteError)]
        [InlineData("get-vanilla")]
        [InlineData("get-vanilla-empty-query-key")]
        [InlineData("get-vanilla-query")]
        [InlineData("get-vanilla-query-order-key")]
        [InlineData("get-vanilla-query-order-key-case")]
        [InlineData("get-vanilla-query-order-value")]
        [InlineData("get-vanilla-query-unreserved")]
        [InlineData("get-vanilla-utf8-query")]
        [InlineData("normalize-path", "get-relative")]
        [InlineData("normalize-path", "get-relative-relative")]
        [InlineData("normalize-path", "get-slash")]
        [InlineData("normalize-path", "get-slash-dot-slash")]
        [InlineData("normalize-path", "get-slashes")]
        [InlineData("normalize-path", "get-slash-pointless-dot")]
        [InlineData("normalize-path", "get-space", Skip = SkipReasons.PlausibleCanonicalUriTestSuiteError)]
        [InlineData("post-header-key-case")]
        [InlineData("post-header-key-sort")]
        [InlineData("post-header-value-case")]
        [InlineData("post-sts-token", "post-sts-header-after")]
        [InlineData("post-sts-token", "post-sts-header-before")]
        [InlineData("post-vanilla")]
        [InlineData("post-vanilla-empty-query-value")]
        [InlineData("post-vanilla-query")]
        [InlineData("post-x-www-form-urlencoded", Skip = SkipReasons.RedundantContentTypeCharset)]
        [InlineData("post-x-www-form-urlencoded-parameters", Skip = SkipReasons.RedundantContentTypeCharset)]
        public async Task PassTestSuite(params string[] scenarioName)
        {
            // Arrange
            var scenario = context.LoadScenario(scenarioName);

            // Act
            var actual = await Signer.SignAsync(
                scenario.Request,
                httpClient.BaseAddress,
                httpClient.DefaultRequestHeaders,
                context.UtcNow,
                context.RegionName,
                context.ServiceName,
                context.Credentials);

            // Assert
            actual.CanonicalRequest.ShouldBe(scenario.ExpectedCanonicalRequest);
            actual.StringToSign.ShouldBe(scenario.ExpectedStringToSign);
            actual.AuthorizationHeader.ShouldBe(scenario.ExpectedAuthorizationHeader);

            scenario.Request.Headers.GetValues("Authorization").Single().ShouldBe(scenario.ExpectedAuthorizationHeader);
        }

        [Theory]
        [InlineData("post-vanilla")]
        public void CompleteSynchronouslyGivenAsyncFalse(params string[] scenarioName)
        {
            // Arrange
            var scenario = context.LoadScenario(scenarioName);

            // Act
            var signingTask = Signer.SignAsync(
                scenario.Request,
                httpClient.BaseAddress,
                httpClient.DefaultRequestHeaders,
                context.UtcNow,
                context.RegionName,
                context.ServiceName,
                context.Credentials,
                async: false);

            // Assert
            Assert.Equal(TaskStatus.RanToCompletion, signingTask.Status);

            scenario.Request.Headers.GetValues("Authorization").Single().ShouldBe(scenario.ExpectedAuthorizationHeader);
        }

        [Theory]
        [InlineData("https://github.com/FantasticFiasco", null, "https://github.com/FantasticFiasco")]
        [InlineData("https://github.com/", "FantasticFiasco", "https://github.com/FantasticFiasco")]
        [InlineData("https://github.com", "/FantasticFiasco", "https://github.com/FantasticFiasco")]
        [InlineData("https://github.com/", "/FantasticFiasco", "https://github.com/FantasticFiasco")]
        [InlineData(null, "https://github.com/FantasticFiasco", "https://github.com/FantasticFiasco")]
        public async Task RespectBaseAddress(string baseAddress, string requestUri, string expectedRequestUri)
        {
            // Arrange
            httpClient.BaseAddress = baseAddress != null
                ? new Uri(baseAddress)
                : null;

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            // Act
            await Signer.SignAsync(
                request,
                httpClient.BaseAddress,
                httpClient.DefaultRequestHeaders,
                context.UtcNow,
                context.RegionName,
                context.ServiceName,
                context.Credentials);

            // Assert
            request.RequestUri.ShouldBe(new Uri(expectedRequestUri));
        }

        /// <summary>
        /// Only requests to S3 should add the "X-Amz-Content-SHA256" header.
        /// </summary>
        [Fact]
        public async Task NotAddXAmzContentHeader()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "https://github.com/FantasticFiasco");

            // Act
            await Signer.SignAsync(
                request,
                httpClient.BaseAddress,
                httpClient.DefaultRequestHeaders,
                context.UtcNow,
                context.RegionName,
                context.ServiceName,
                context.Credentials);

            // Assert
            request.Headers.Contains("X-Amz-Content-SHA256").ShouldBeFalse();
        }

        [Fact]
        public async Task ThrowArgumentExceptionGivenXAmzDateHeader()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "https://github.com/FantasticFiasco");
            request.AddHeader(HeaderKeys.XAmzDateHeader, "some value");

            // Act
            var actual = Signer.SignAsync(
                request,
                httpClient.BaseAddress,
                httpClient.DefaultRequestHeaders,
                context.UtcNow,
                context.RegionName,
                context.ServiceName,
                context.Credentials);

            // Assert
            await actual.ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task ThrowArgumentExceptionGivenAuthorizationHeader()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "https://github.com/FantasticFiasco");
            request.Headers.Authorization = new AuthenticationHeaderValue("Some-Schema");

            // Act
            var actual = Signer.SignAsync(
                request,
                httpClient.BaseAddress,
                httpClient.DefaultRequestHeaders,
                context.UtcNow,
                context.RegionName,
                context.ServiceName,
                context.Credentials);

            // Assert
            await actual.ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task ThrowArgumentExceptionGivenAuthorizationHeaderAddedByName()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "https://github.com/FantasticFiasco");
            request.AddHeader(HeaderKeys.AuthorizationHeader, "some value");

            // Act
            var actual = Signer.SignAsync(
                request,
                httpClient.BaseAddress,
                httpClient.DefaultRequestHeaders,
                context.UtcNow,
                context.RegionName,
                context.ServiceName,
                context.Credentials);

            // Assert
            await actual.ShouldThrowAsync<ArgumentException>();
        }

        public void Dispose()
        {
            httpClient?.Dispose();
            context.ResetHeaderValueSeparator();
        }
    }
}
