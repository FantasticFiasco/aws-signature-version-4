using System.Net;
using System.Net.Http;
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
        public async Task Succeed(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var url = $"{Context.S3Url}/foo.txt";

            // Act
            var response = await HttpClient.GetAsync(
                url,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
