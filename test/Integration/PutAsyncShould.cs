//using System.Threading.Tasks;
//using AWS.SignatureVersion4.Integration;
//using AWS.SignatureVersion4.Integration.Authentication;
//using AWS.SignatureVersion4.Integration.Contents;
//using Shouldly;
//using Xunit;
//
//// ReSharper disable once CheckNamespace
//namespace System.Net.Http
//{
//    public class PutAsyncShould : IntegrationBase
//    {
//        public PutAsyncShould(IntegrationTestContext context)
//            : base(context)
//        {
//        }
//
//        [Theory]
//        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
//        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
//        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
//        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
//        public async Task Succeed(IamAuthenticationType iamAuthenticationType, Type contentType)
//        {
//            // Act
//            var response = await HttpClient.PutAsync(
//                Context.ApiGatewayUrl,
//                contentType.ToContent(),
//                Context.RegionName,
//                Context.ServiceName,
//                ResolveCredentials(iamAuthenticationType));
//
//            // Assert
//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//        }
//
//        [Theory]
//        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
//        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
//        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
//        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
//        public async Task SucceedGivenQuery(IamAuthenticationType iamAuthenticationType, Type contentType)
//        {
//            // Arrange
//            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
//            {
//                Query = "Param1=value1"
//            };
//
//            // Act
//            var response = await HttpClient.PutAsync(
//                uriBuilder.Uri,
//                contentType.ToContent(),
//                Context.RegionName,
//                Context.ServiceName,
//                ResolveCredentials(iamAuthenticationType));
//
//            // Assert
//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//        }
//
//        [Theory]
//        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
//        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
//        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
//        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
//        public async Task SucceedGivenOrderedQuery(IamAuthenticationType iamAuthenticationType, Type contentType)
//        {
//            // Arrange
//            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
//            {
//                Query = "Param1=Value1&Param1=value2"
//            };
//
//            // Act
//            var response = await HttpClient.PutAsync(
//                uriBuilder.Uri,
//                contentType.ToContent(),
//                Context.RegionName,
//                Context.ServiceName,
//                ResolveCredentials(iamAuthenticationType));
//
//            // Assert
//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//        }
//
//        [Theory]
//        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
//        [InlineData(IamAuthenticationType.User, typeof(RichContent))]
//        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
//        [InlineData(IamAuthenticationType.Role, typeof(RichContent))]
//        public async Task SucceedGivenUnorderedQuery(IamAuthenticationType iamAuthenticationType, Type contentType)
//        {
//            // Arrange
//            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
//            {
//                Query = "Param1=value2&Param1=Value1"
//            };
//
//            // Act
//            var response = await HttpClient.PutAsync(
//                uriBuilder.Uri,
//                contentType.ToContent(),
//                Context.RegionName,
//                Context.ServiceName,
//                ResolveCredentials(iamAuthenticationType));
//
//            // Assert
//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//        }
//    }
//}
