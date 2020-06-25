using System;
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
        private readonly HttpContent content;
        private readonly string now;

        public PutAsyncShould(IntegrationTestContext context)
            : base(context)
        {
            content = new StringContent("This is some content...");
            now = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenNoPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = GenerateRandomKey();

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                content,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = GenerateRandomKey("put/");

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                content,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenDeepPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = GenerateRandomKey("put/deep/");

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                content,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.Lowercase.Name)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.Lowercase.Name)]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.Numbers.Name)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.Numbers.Name)]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.SpecialCharacters.Name)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.SpecialCharacters.Name)]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.Uppercase.Name)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.Uppercase.Name)]
        public async Task SucceedGivenSafeCharacters(IamAuthenticationType iamAuthenticationType, string characters)
        {
            // Arrange
            var key = GenerateRandomKey($"put/{characters}-");

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                content,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenCancellationToken(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = GenerateRandomKey();
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                content,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public void AbortGivenCanceled(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = GenerateRandomKey();
            var ct = new CancellationToken(true);

            // Act
            var task = HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                content,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }

        private string GenerateRandomKey(string prefix = null)
        {
            var id = Guid.NewGuid().ToString();

            return $"{prefix}{now}-{id}.txt";
        }
    }
}
