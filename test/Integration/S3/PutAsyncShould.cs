﻿using System;
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
        private readonly string now;

        public PutAsyncShould(IntegrationTestContext context)
            : base(context)
        {
            now = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenNoPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = Bucket.Foo.Key;

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                new StringContent(Bucket.Foo.Content),
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
            var key = Bucket.Foo.Bar.Key;

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                new StringContent(Bucket.Foo.Bar.Content),
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
            var key = Bucket.Foo.Bar.Baz.Key;

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                new StringContent(Bucket.Foo.Bar.Baz.Content),
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
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
            var key = GenerateRandomTempKey($"{characters}-");

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                new StringContent("This is some content..."),
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
            var key = GenerateRandomTempKey();
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                new StringContent("This is some content..."),
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
            var key = GenerateRandomTempKey();
            var ct = new CancellationToken(true);

            // Act
            var task = HttpClient.PutAsync(
                $"{Context.S3Url}{key}",
                new StringContent("This is some content..."),
                ct,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }

        private string GenerateRandomTempKey(string namePrefix = null)
        {
            var id = Guid.NewGuid().ToString();

            return $"temp/{namePrefix}{now}-{id}.txt";
        }
    }
}
