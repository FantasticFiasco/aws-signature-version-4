using System;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Runtime;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite.Fixtures;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit.Private
{
    public class SignerGivenS3Should : IClassFixture<TestSuiteFixture>, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly DateTime utcNow;
        private readonly string region;
        private readonly string serviceName;
        private readonly ImmutableCredentials immutableCredentials;

        public SignerGivenS3Should(TestSuiteFixture fixture)
        {
            httpClient = new HttpClient();
            utcNow = fixture.UtcNow;
            region = fixture.Region.SystemName;
            serviceName = "s3";
            immutableCredentials = fixture.ImmutableCredentials;
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
                utcNow,
                region,
                serviceName,
                immutableCredentials);

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
                utcNow,
                region,
                serviceName,
                immutableCredentials);

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
