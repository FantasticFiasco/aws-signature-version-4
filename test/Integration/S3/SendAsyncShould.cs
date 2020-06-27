using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.S3
{
    public class SendAsyncShould : S3IntegrationBase
    {
        public SendAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }
        
        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        public async Task SucceedGivenHttpCompletionOption(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var requestUri = $"{Context.S3Url}{Bucket.Foo.Key}";
            var request = new HttpRequestMessage(new HttpMethod(method), requestUri);
            var completionOption = HttpCompletionOption.ResponseContentRead;

            // Act
            var response = await HttpClient.SendAsync(
                request,
                completionOption,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
