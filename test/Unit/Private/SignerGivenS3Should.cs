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

        [Fact]
        public async Task AddXAmzContentHeader()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "https://github.com/FantasticFiasco");

            // Act
            await Signer.SignAsync(
                httpClient,
                request,
                context.UtcNow,
                context.RegionName,
                ServiceName.S3,
                context.Credentials);

            // Assert
            request.Headers.Contains("X-Amz-Content-SHA256").ShouldBeTrue();
        }

        public void Dispose()
        {
            httpClient?.Dispose();
            context.ResetHeaderValueSeparator();
        }
    }
}
