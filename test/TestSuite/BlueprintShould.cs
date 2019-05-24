using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.TestSuite
{
    public class BlueprintShould : IDisposable
    {
        private const string Url = "https://docs.aws.amazon.com/general/latest/gr/samples/aws-sig-v4-test-suite.zip";
        private const string TestSuiteHash = "f41524defefb645d5709b5b7de5e30da";

        private readonly HttpClient client;
        private readonly MD5 md5;

        public BlueprintShould()
        {
            client = new HttpClient();
            md5 = MD5.Create();
        }

        /// <summary>
        /// Make sure that the test suite blueprint we base our tests on is the latest suite
        /// released by AWS. If not, we have to download the latest version and make sure the tests
        /// still pass.
        /// </summary>
        [Fact]
        public async Task BeUpToDate()
        {
            // Arrange
            using (var stream = await client.GetStreamAsync(Url))
            {
                // Act
                var hash = CreateHash(stream);

                // Assert
                hash.ShouldBe(TestSuiteHash);
            }
        }

        private string CreateHash(Stream stream)
        {
            var hash = md5.ComputeHash(stream);

            return BitConverter.ToString(hash)
                .Replace("-", string.Empty)
                .ToLowerInvariant();
        }

        public void Dispose()
        {
            client?.Dispose();
            md5?.Dispose();
        }
    }
}
