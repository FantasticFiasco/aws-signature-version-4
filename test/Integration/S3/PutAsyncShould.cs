using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.S3
{
    public class PutAsyncShould : S3IntegrationBase
    {
        public PutAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenNoPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = new BucketObject(BucketObjectKey.WithoutPrefix);

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                bucketObject.StringContent,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenSingleLevelPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = new BucketObject(BucketObjectKey.WithSingleLevelPrefix);

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                bucketObject.StringContent,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenMultiLevelPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = new BucketObject(BucketObjectKey.WithMultiLevelPrefix);

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                bucketObject.StringContent,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
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
            var bucketObject = new BucketObject(key);

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                bucketObject.StringContent,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenCharactersThatRequireSpecialHandling(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = new BucketObject(BucketObjectKey.WithCharactersThatRequireSpecialHandling);

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                bucketObject.StringContent,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenUnnormalizedDelimiters(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = new BucketObject(BucketObjectKey.WithUnnormalizedDelimiter);

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                bucketObject.StringContent,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenCancellationToken(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = new BucketObject(BucketObjectKey.WithoutPrefix);
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                bucketObject.StringContent,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task AbortGivenCanceled(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = new BucketObject(BucketObjectKey.WithoutPrefix);
            var ct = new CancellationToken(true);
            
            // Act
            var task = HttpClient.PutAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}",
                bucketObject.StringContent,
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
