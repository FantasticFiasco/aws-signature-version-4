using System;
using System.Net.Http;
using System.Threading.Tasks;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite.Fixtures;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit.Private
{
    public class SignerGivenS3Should : IClassFixture<TestSuiteFixture>, IDisposable
    {
        private readonly TestSuiteFixture fixture;
        private readonly HttpClient httpClient;

        public SignerGivenS3Should(TestSuiteFixture fixture)
        {
            this.fixture = fixture;

            httpClient = new HttpClient();
        }

        #region Add X-Amz-Content-SHA256 header

        [Fact]
        public async Task AddXAmzContentHeaderAsync()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "https://github.com/FantasticFiasco");

            // Act
            await Signer.SignAsync(
                request,
                httpClient.BaseAddress,
                httpClient.DefaultRequestHeaders,
                fixture.UtcNow,
                fixture.Region.SystemName,
                "s3",
                fixture.ImmutableCredentials);

            // Assert
            request.Headers.Contains("X-Amz-Content-SHA256").ShouldBeTrue();
        }

        [Fact]
        public void AddXAmzContentHeader()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "https://github.com/FantasticFiasco");

            // Act
            Signer.Sign(
                request,
                httpClient.BaseAddress,
                httpClient.DefaultRequestHeaders,
                fixture.UtcNow,
                fixture.Region.SystemName,
                "s3",
                fixture.ImmutableCredentials);

            // Assert
            request.Headers.Contains("X-Amz-Content-SHA256").ShouldBeTrue();
        }

        #endregion

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
