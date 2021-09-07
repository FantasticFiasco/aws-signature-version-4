using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.TestSuite;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.S3
{
    public class SendShould : S3IntegrationBase, IClassFixture<TestSuiteContext>
    {
        private readonly TestSuiteContext testSuiteContext;

        public SendShould(IntegrationTestContext context, TestSuiteContext testSuiteContext)
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
            var request = SendAsyncShould.BuildRequest(testSuiteContext, Context, scenarioName);
            var iamAuthenticationType = IamAuthenticationType.User;

            await SendAsyncShould.UploadRequiredObjectAsync(Bucket, scenarioName);

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
            var request = SendAsyncShould.BuildRequest(testSuiteContext, Context, scenarioName);
            var iamAuthenticationType = IamAuthenticationType.Role;

            await SendAsyncShould.UploadRequiredObjectAsync(Bucket, scenarioName);

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
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOption(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithoutPrefix);
            var requestUri = $"{Context.S3BucketUrl}/{bucketObject.Key}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
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
    }
}
