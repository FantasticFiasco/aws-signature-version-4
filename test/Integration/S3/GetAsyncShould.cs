using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.S3
{
    public class GetAsyncShould : S3IntegrationBase
    {
        public GetAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenNoPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithoutPrefix);

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(bucketObject.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenSingleLevelPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithSingleLevelPrefix);

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(bucketObject.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenMultiLevelPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithMultiLevelPrefix);

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(bucketObject.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, BucketObjectKey.WithLowercaseSafeCharacters)]
        [InlineData(IamAuthenticationType.Role, BucketObjectKey.WithLowercaseSafeCharacters)]
        [InlineData(IamAuthenticationType.User, BucketObjectKey.WithNumberSafeCharacters)]
        [InlineData(IamAuthenticationType.Role, BucketObjectKey.WithNumberSafeCharacters)]
        [InlineData(IamAuthenticationType.User, BucketObjectKey.WithSpecialSafeCharacters)]
        [InlineData(IamAuthenticationType.Role, BucketObjectKey.WithSpecialSafeCharacters)]
        [InlineData(IamAuthenticationType.User, BucketObjectKey.WithUppercaseSafeCharacters)]
        [InlineData(IamAuthenticationType.Role, BucketObjectKey.WithUppercaseSafeCharacters)]
        public async Task SucceedGivenSafeCharacters(IamAuthenticationType iamAuthenticationType, string key)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(key);

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(bucketObject.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenCharactersThatRequireSpecialHandling(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithCharactersThatRequireSpecialHandling);

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(bucketObject.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenUnnormalizedDelimiters(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithUnnormalizedDelimiter);

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(bucketObject.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOption(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithoutPrefix);
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(bucketObject.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenCancellationToken(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithoutPrefix);
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(bucketObject.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOptionAndCancellationToken(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = await Bucket.PutObjectAsync(BucketObjectKey.WithoutPrefix);
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(bucketObject.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task ReturnNotFoundGivenUnknownKey(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = "unknown.txt";

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task AbortGivenCanceled(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = BucketObjectKey.WithoutPrefix;
            var ct = new CancellationToken(true);

            // Act
            var task = HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{key}",
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
    }
}
