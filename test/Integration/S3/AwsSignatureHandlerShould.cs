using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.S3
{
    public class AwsSignatureHandlerShould : S3IntegrationBase, IClassFixture<TestSuiteContext>
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
        [InlineData("get-vanilla-query-unreserved")]
        [InlineData("get-vanilla-utf8-query")]
        [InlineData("normalize-path", "get-relative")]
        [InlineData("normalize-path", "get-relative-relative")]
        [InlineData("normalize-path", "get-slash")]
        [InlineData("normalize-path", "get-slash-dot-slash")]
        [InlineData("normalize-path", "get-slashes")]
        [InlineData("normalize-path", "get-slash-pointless-dot", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("normalize-path", "get-space")]
        [InlineData("post-header-key-case", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-header-key-sort", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-header-value-case", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-sts-token", "post-sts-header-after", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-sts-token", "post-sts-header-before", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-vanilla", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-vanilla-empty-query-value", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-vanilla-query", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-x-www-form-urlencoded", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-x-www-form-urlencoded-parameters", Skip = SkipReasons.NotSupportedByS3)]
        public async Task PassTestSuiteGivenUserWithPermissions(params string[] scenarioName)
        {
            // Arrange
            using var httpClient = HttpClientFactory(IamAuthenticationType.User).CreateClient("integration");
            var request = BuildRequest(scenarioName);

            await UploadRequiredObjectAsync(scenarioName);

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
        [InlineData("get-vanilla-query-unreserved")]
        [InlineData("get-vanilla-utf8-query")]
        [InlineData("normalize-path", "get-relative")]
        [InlineData("normalize-path", "get-relative-relative")]
        [InlineData("normalize-path", "get-slash")]
        [InlineData("normalize-path", "get-slash-dot-slash")]
        [InlineData("normalize-path", "get-slashes")]
        [InlineData("normalize-path", "get-slash-pointless-dot", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("normalize-path", "get-space")]
        [InlineData("post-header-key-case", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-header-key-sort", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-header-value-case", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-sts-token", "post-sts-header-after", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-sts-token", "post-sts-header-before", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-vanilla", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-vanilla-empty-query-value", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-vanilla-query", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-x-www-form-urlencoded", Skip = SkipReasons.NotSupportedByS3)]
        [InlineData("post-x-www-form-urlencoded-parameters", Skip = SkipReasons.NotSupportedByS3)]
        public async Task PassTestSuiteGivenAssumedRole(params string[] scenarioName)
        {
            // Arrange
            using var httpClient = HttpClientFactory(IamAuthenticationType.Role).CreateClient("integration");
            var request = BuildRequest(scenarioName);

            await UploadRequiredObjectAsync(scenarioName);

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOption(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithoutPrefix);

            using var httpClient = HttpClientFactory(iamAuthenticationType).CreateClient("integration");
            var requestUri = $"{Context.S3BucketUrl}/{bucketObject.Key}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await httpClient.SendAsync(request, completionOption);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private async Task UploadRequiredObjectAsync(params string[] scenarioName)
        {
            if (scenarioName[0] == "get-unreserved" || scenarioName[0] == "get-vanilla-query-unreserved")
            {
                await Bucket.PutObjectAsync("-._~0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
            }
            else if (scenarioName[0] == "get-utf8" || scenarioName[0] == "get-vanilla-utf8-query")
            {
                await Bucket.PutObjectAsync("ሴ");
            }
            else if (scenarioName.Length == 2 && scenarioName[0] == "normalize-path" && scenarioName[1] == "get-slashes")
            {
                await Bucket.PutObjectAsync("/example//");
            }
            else if (scenarioName.Length == 2 && scenarioName[0] == "normalize-path" && scenarioName[1] == "get-space")
            {
                await Bucket.PutObjectAsync("example space/");
            }
            else
            {
                await Bucket.PutObjectAsync("/");
            }
        }

        private HttpRequestMessage BuildRequest(string[] scenarioName)
        {
            var request = testSuiteContext.LoadScenario(scenarioName).Request;

            // Redirect the request to the AWS S3 bucket
            request.RequestUri = request.RequestUri
                .ToString()
                .Replace("https://example.amazonaws.com", Context.S3BucketUrl)
                .ToUri();

            // The "Host" header is now invalid since we redirected the request to the AWS S3
            // bucket. Lets remove the header and have the signature implementation re-add it
            // correctly.
            request.Headers.Remove("Host");

            return request;
        }
    }
}
