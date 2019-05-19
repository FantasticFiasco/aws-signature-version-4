using System.Threading.Tasks;
using AWS.SignatureVersion4.Private;
using AWS.SignatureVersion4.Integration;
using AWS.SignatureVersion4.Integration.Authentication;
using Shouldly;
using Xunit;

// ReSharper disable once CheckNamespace
namespace System.Net.Http
{
    public class SendAsyncShould : IntegrationBase
    {
        public SendAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenHeaderWithDuplicateValues(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value2", "value2" });

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenHeaderWithUnorderedValues(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value4", "value1", "value3", "value2" });

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenHeaderWithWhitespaceCharacters(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), Context.ApiGatewayUrl);
            request.AddHeaders("My-Header1", new[] { "value1", "a   b   c" });

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenQuery(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=value1"
            };

            var request = new HttpRequestMessage(new HttpMethod(method), uriBuilder.Uri);

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenOrderedQuery(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=value2"
            };

            var request = new HttpRequestMessage(new HttpMethod(method), uriBuilder.Uri);

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, "GET")]
        [InlineData(IamAuthenticationType.User, "POST")]
        [InlineData(IamAuthenticationType.User, "PUT")]
        [InlineData(IamAuthenticationType.User, "DELETE")]
        [InlineData(IamAuthenticationType.Role, "GET")]
        [InlineData(IamAuthenticationType.Role, "POST")]
        [InlineData(IamAuthenticationType.Role, "PUT")]
        [InlineData(IamAuthenticationType.Role, "DELETE")]
        public async Task SucceedGivenUnorderedQuery(
            IamAuthenticationType iamAuthenticationType,
            string method)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=value2&Param1=Value1"
            };

            var request = new HttpRequestMessage(new HttpMethod(method), uriBuilder.Uri);

            // Act
            var response = await HttpClient.SendAsync(
                request,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
