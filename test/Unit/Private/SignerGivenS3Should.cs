using System;
using System.Net.Http;
using System.Threading.Tasks;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit.Private
{
    public class SignerGivenS3Should : IClassFixture<TestSuiteContext>, IDisposable
    {
        private readonly TestSuiteContext context;
        private readonly HttpClient httpClient;

        public SignerGivenS3Should(TestSuiteContext context)
        {
            this.context = context;

            httpClient = new HttpClient();

            context.AdjustHeaderValueSeparator();
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
                context.UtcNow,
                context.RegionName,
                "s3",
                context.Credentials);

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
                context.UtcNow,
                context.RegionName,
                "s3",
                context.Credentials);

            // Assert
            request.Headers.Contains("X-Amz-Content-SHA256").ShouldBeTrue();
        }

        #endregion

        public void Dispose()
        {
            httpClient?.Dispose();
            context.ResetHeaderValueSeparator();
        }
    }
}
