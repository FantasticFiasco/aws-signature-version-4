﻿using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.S3
{
    public class DeleteAsyncShould : S3IntegrationBase
    {
        public DeleteAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenNoPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var requestUri = await CreateObject(iamAuthenticationType, "delete.txt");

            // Act
            var response = await HttpClient.DeleteAsync(
                requestUri,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var requestUri = await CreateObject(iamAuthenticationType, "temp/delete.txt");

            // Act
            var response = await HttpClient.DeleteAsync(
                requestUri,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenDeepPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var requestUri = await CreateObject(iamAuthenticationType, "temp/deep/delete.txt");

            // Act
            var response = await HttpClient.DeleteAsync(
                requestUri,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.Lowercase.NameWithoutExtension)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.Lowercase.NameWithoutExtension)]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.Numbers.NameWithoutExtension)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.Numbers.NameWithoutExtension)]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.SpecialCharacters.NameWithoutExtension)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.SpecialCharacters.NameWithoutExtension)]
        [InlineData(IamAuthenticationType.User, Bucket.SafeCharacters.Uppercase.NameWithoutExtension)]
        [InlineData(IamAuthenticationType.Role, Bucket.SafeCharacters.Uppercase.NameWithoutExtension)]
        public async Task SucceedGivenSafeCharacters(IamAuthenticationType iamAuthenticationType, string characters)
        {
            // Arrange
            var requestUri = await CreateObject(iamAuthenticationType, $"temp/delete-{characters}.txt");

            // Act
            var response = await HttpClient.DeleteAsync(
                requestUri,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenCancellationToken(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var requestUri = await CreateObject(iamAuthenticationType, "temp/delete.txt");
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.DeleteAsync(
                requestUri,
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public void AbortGivenCanceled(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = "temp/delete.txt";
            var ct = new CancellationToken(true);

            // Act
            var task = HttpClient.DeleteAsync(
                $"{Context.S3Url}{key}",
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }

        private async Task<string> CreateObject(IamAuthenticationType iamAuthenticationType, string key)
        {
            var requestUri = $"{Context.S3Url}{key}";

            var response = await HttpClient.PutAsync(
                requestUri,
                new StringContent("This is some content..."),
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            response.StatusCode.ShouldBe(HttpStatusCode.OK, "Arranging test by creating S3 object failed");

            return requestUri;
        }
    }
}
