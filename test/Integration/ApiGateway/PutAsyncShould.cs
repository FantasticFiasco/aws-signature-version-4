using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using AwsSignatureVersion4.Integration.ApiGateway.Contents;
using AwsSignatureVersion4.Integration.ApiGateway.Requests;
using AwsSignatureVersion4.Private;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway
{
    public class PutAsyncShould : ApiGatewayIntegrationBase
    {
        public PutAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        #region PutAsync(string, HttpContent, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenRequestStringAndMutableCredentials(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Act
            var response = await HttpClient.PutAsync(
                Context.ApiGatewayUrl,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenRequestStringAndImmutableCredentials(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Act
            var response = await HttpClient.PutAsync(
                Context.ApiGatewayUrl,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        #endregion

        #region PutAsync(Uri, HttpContent, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenRequestUriAndMutableCredentials(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Act
            var response = await HttpClient.PutAsync(
                Context.ApiGatewayUrl.ToUri(),
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenRequestUriAndImmutableCredentials(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Act
            var response = await HttpClient.PutAsync(
                Context.ApiGatewayUrl.ToUri(),
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        #endregion

        #region PutAsync(string, HttpContent, CancellationToken, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.PutAsync(
                Context.ApiGatewayUrl,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenRequestStringAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.PutAsync(
                Context.ApiGatewayUrl,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenCancellationToken(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.PutAsync(
                Context.ApiGatewayUrl,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task AbortGivenCanceled(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var ct = new CancellationToken(true);

            // Act
            var task = HttpClient.PutAsync(
                Context.ApiGatewayUrl,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            while (task.Status == TaskStatus.WaitingForActivation)
            {
                await Task.Delay(1);
            }

            // Assert
            task.Status.ShouldBe(TaskStatus.Canceled);
        }

        #endregion

        #region PutAsync(Uri, HttpContent, CancellationToken, string, string, <credentials>)

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndMutableCredentials(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.PutAsync(
                Context.ApiGatewayUrl.ToUri(),
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenRequestUriAndCancellationTokenAndImmutableCredentials(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var ct = new CancellationToken();

            // Act
            var response = await HttpClient.PutAsync(
                Context.ApiGatewayUrl.ToUri(),
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveImmutableCredentials(iamAuthenticationType),
                ct);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBeNull();
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        #endregion

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenQuery(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1"
            };

            // Act
            var response = await HttpClient.PutAsync(
                uriBuilder.Uri,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBe(
                new Dictionary<string, string>
                {
                    ["Param1"] = "Value1"
                });
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        [Theory]
        [InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        [InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        [InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        public async Task SucceedGivenOrderedQuery(IamAuthenticationType iamAuthenticationType, Type contentType)
        {
            // Arrange
            var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
            {
                Query = "Param1=Value1&Param1=Value2"
            };

            // Act
            var response = await HttpClient.PutAsync(
                uriBuilder.Uri,
                contentType.ToJsonContent(),
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var receivedRequest = await response.Content.ReadReceivedRequestAsync();
            receivedRequest.Method.ShouldBe("PUT");
            receivedRequest.Path.ShouldBe("/");
            receivedRequest.QueryParameters.ShouldBe(
                new Dictionary<string, string>
                {
                    ["Param1"] = "Value1",
                    ["Param1"] = "Value2"
                });
            receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        }

        //[Theory]
        //[InlineData(IamAuthenticationType.User, typeof(EmptyContent))]
        //[InlineData(IamAuthenticationType.User, typeof(JsonContent))]
        //[InlineData(IamAuthenticationType.Role, typeof(EmptyContent))]
        //[InlineData(IamAuthenticationType.Role, typeof(JsonContent))]
        //public async Task SucceedGivenUnorderedQuery(IamAuthenticationType iamAuthenticationType, Type contentType)
        //{
        //    // Arrange
        //    var uriBuilder = new UriBuilder(Context.ApiGatewayUrl)
        //    {
        //        Query = "Param1=Value2&Param1=Value1"
        //    };

        //    // Act
        //    var response = await HttpClient.PutAsync(
        //        uriBuilder.Uri,
        //        contentType.ToJsonContent(),
        //        Context.RegionName,
        //        Context.ServiceName,
        //        ResolveMutableCredentials(iamAuthenticationType));

        //    // Assert
        //    response.StatusCode.ShouldBe(HttpStatusCode.OK);

        //    var receivedRequest = await response.Content.ReadReceivedRequestAsync();
        //    receivedRequest.Method.ShouldBe("PUT");
        //    receivedRequest.Path.ShouldBe("/");
        //    receivedRequest.QueryParameters.ShouldBe(
        //        new Dictionary<string, string>
        //        {
        //            ["Param1"] = "Value2",
        //            ["Param1"] = "Value1"
        //        });
        //    receivedRequest.Body.ShouldBe(contentType.ToJsonString());
        //}
    }
}
