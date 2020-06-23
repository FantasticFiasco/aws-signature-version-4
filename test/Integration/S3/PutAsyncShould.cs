using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Util;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.S3
{
    public class PutAsyncShould : S3IntegrationBase
    {
        private readonly HttpContent content;

        public PutAsyncShould(IntegrationTestContext context)
            : base(context)
        {
            content = new StringContent("This is some content....");
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedGivenNoPrefix(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var key = GenerateKey();

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

        private static string GenerateKey()
        {
            var now = DateTime.Now.ToString("yyyyMMdd HHmmss");
            var id = Guid.NewGuid().ToString();

            return AWSSDKUtils.UrlEncode($"{now} {id}.txt", false);
        }
    }
}
