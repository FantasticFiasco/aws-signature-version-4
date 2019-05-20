using System.Threading.Tasks;
using AWS.SignatureVersion4.Integration;
using AWS.SignatureVersion4.Integration.Authentication;
using Shouldly;
using Xunit;

// ReSharper disable once CheckNamespace
namespace System.Net.Http
{
    public class GetAsyncShould : IntegrationBase
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
            // Act
            var response = await HttpClient.GetAsync(
                Context.ApiGatewayUrl,
                Context.RegionName,
                Context.ServiceName,
                ResolveCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

//        [Theory]
//        [InlineData(IamAuthenticationType.User)]
//        [InlineData(IamAuthenticationType.Role)]
//        public async Task SucceedGivenQuery(IamAuthenticationType iamAuthenticationType)
//        {
//            // Arrange
//            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
//            {
//                Query = "Param1=value1"
//            };
//
//            // Act
//            var response = await HttpClient.GetAsync(
//                uriBuilder.Uri,
//                Context.RegionName,
//                Context.ServiceName,
//                ResolveCredentials(iamAuthenticationType));
//
//            // Assert
//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//        }
//
//        [Theory]
//        [InlineData(IamAuthenticationType.User)]
//        [InlineData(IamAuthenticationType.Role)]
//        public async Task SucceedGivenOrderedQuery(IamAuthenticationType iamAuthenticationType)
//        {
//            // Arrange
//            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
//            {
//                Query = "Param1=Value1&Param1=value2"
//            };
//
//            // Act
//            var response = await HttpClient.GetAsync(
//                uriBuilder.Uri,
//                Context.RegionName,
//                Context.ServiceName,
//                ResolveCredentials(iamAuthenticationType));
//
//            // Assert
//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//        }
//
//        [Theory]
//        [InlineData(IamAuthenticationType.User)]
//        [InlineData(IamAuthenticationType.Role)]
//        public async Task SucceedGivenUnorderedQuery(IamAuthenticationType iamAuthenticationType)
//        {
//            // Arrange
//            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
//            {
//                Query = "Param1=value2&Param1=Value1"
//            };
//
//            // Act
//            var response = await HttpClient.GetAsync(
//                uriBuilder.Uri,
//                Context.RegionName,
//                Context.ServiceName,
//                ResolveCredentials(iamAuthenticationType));
//
//            // Assert
//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//        }
    }
}
