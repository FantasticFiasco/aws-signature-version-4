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
            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3Url}{Bucket.Foo.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(Bucket.Foo.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3Url}{Bucket.Foo.Bar.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(Bucket.Foo.Bar.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenDeepPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3Url}{Bucket.Foo.Bar.Baz.Key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(Bucket.Foo.Bar.Baz.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.Lowercase.Key, Bucket.SafeCharacters.Lowercase.Content)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.Lowercase.Key, Bucket.SafeCharacters.Lowercase.Content)]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.Numbers.Key, Bucket.SafeCharacters.Numbers.Content)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.Numbers.Key, Bucket.SafeCharacters.Numbers.Content)]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.SpecialCharacters.Key, Bucket.SafeCharacters.SpecialCharacters.Content)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.SpecialCharacters.Key, Bucket.SafeCharacters.SpecialCharacters.Content)]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.Uppercase.Key, Bucket.SafeCharacters.Uppercase.Content)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.Uppercase.Key, Bucket.SafeCharacters.Uppercase.Content)]
        public async Task SucceedGivenSafeCharacters(IamAuthenticationType iamAuthenticationType, string key, string expectedContent)
        {
            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3Url}{key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(expectedContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOption(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3Url}{Bucket.Foo.Key}",
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(Bucket.Foo.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenCancellationToken(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3Url}{Bucket.Foo.Key}",
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(Bucket.Foo.Content);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenHttpCompletionOptionAndCancellationToken(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var completionOption = HttpCompletionOption.ResponseContentRead;
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.GetAsync(
                $"{Context.S3Url}{Bucket.Foo.Key}",
                completionOption,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).ShouldBe(Bucket.Foo.Content);
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
                $"{Context.S3Url}{key}",
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public void AbortGivenCanceled(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var ct = new CancellationToken(true);

            // Act
            var task = HttpClient.GetAsync(
                $"{Context.S3Url}{Bucket.Foo.Key}",
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }
    }
}
