using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Util;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit
{
    public class AwsSignatureHandlerShould
    {
        private readonly SinkHandler sinkHandler;

        public AwsSignatureHandlerShould()
        {
            sinkHandler = new SinkHandler();
        }

        #region Set headers

        [Theory]
        [InlineData("execute-api")]
        [InlineData("s3")]
        public async Task SetHeadersAsync(string serviceName)
        {
            // Arrange
            var handler = new AwsSignatureHandler(CreateSettings(serviceName))
            {
                InnerHandler = sinkHandler
            };

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri("https://example.amazonaws.com/resource/path"));

            var ct = new CancellationToken();

            // Act
            await InvokeSendAsync(handler, request, ct);

            // Assert
            sinkHandler.Request.Headers.Contains(HeaderKeys.AuthorizationHeader).ShouldBeTrue();
            sinkHandler.Request.Headers.Contains(HeaderKeys.HostHeader).ShouldBeTrue();
            sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzDateHeader).ShouldBeTrue();
            sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzSecurityTokenHeader).ShouldBeTrue();
            if (serviceName == "s3")
            {
                sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzContentSha256Header).ShouldBeTrue();
            }
        }

        [Theory]
        [InlineData("execute-api")]
        [InlineData("s3")]
        public void SetHeaders(string serviceName)
        {
            // Arrange
            var handler = new AwsSignatureHandler(CreateSettings(serviceName))
            {
                InnerHandler = sinkHandler
            };

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri("https://example.amazonaws.com/resource/path"));

            var ct = new CancellationToken();

            // Act
            InvokeSend(handler, request, ct);

            // Assert
            sinkHandler.Request.Headers.Contains(HeaderKeys.AuthorizationHeader).ShouldBeTrue();
            sinkHandler.Request.Headers.Contains(HeaderKeys.HostHeader).ShouldBeTrue();
            sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzDateHeader).ShouldBeTrue();
            sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzSecurityTokenHeader).ShouldBeTrue();
            if (serviceName == "s3")
            {
                sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzContentSha256Header).ShouldBeTrue();
            }
        }

        #endregion

        #region Reset headers

        [Theory]
        [InlineData("execute-api")]
        [InlineData("s3")]
        public async Task ResetHeadersAsync(string serviceName)
        {
            var handler = new AwsSignatureHandler(CreateSettings(serviceName))
            {
                InnerHandler = sinkHandler
            };

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri("https://example.amazonaws.com/resource/path"));

            var ct = new CancellationToken();

            for (var i = 0; i < 2; i++)
            {
                // Act
                await InvokeSendAsync(handler, request, ct);

                // Assert
                sinkHandler.Request.Headers.Contains(HeaderKeys.AuthorizationHeader).ShouldBeTrue();
                sinkHandler.Request.Headers.Contains(HeaderKeys.HostHeader).ShouldBeTrue();
                sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzDateHeader).ShouldBeTrue();
                sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzSecurityTokenHeader).ShouldBeTrue();
                if (serviceName == "s3")
                {
                    sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzContentSha256Header).ShouldBeTrue();
                }
            }
        }

        [Theory]
        [InlineData("execute-api")]
        [InlineData("s3")]
        public void ResetHeaders(string serviceName)
        {
            var handler = new AwsSignatureHandler(CreateSettings(serviceName))
            {
                InnerHandler = sinkHandler
            };

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri("https://example.amazonaws.com/resource/path"));

            var ct = new CancellationToken();

            for (var i = 0; i < 2; i++)
            {
                // Act
                InvokeSend(handler, request, ct);

                // Assert
                sinkHandler.Request.Headers.Contains(HeaderKeys.AuthorizationHeader).ShouldBeTrue();
                sinkHandler.Request.Headers.Contains(HeaderKeys.HostHeader).ShouldBeTrue();
                sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzDateHeader).ShouldBeTrue();
                sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzSecurityTokenHeader).ShouldBeTrue();
                if (serviceName == "s3")
                {
                    sinkHandler.Request.Headers.Contains(HeaderKeys.XAmzContentSha256Header).ShouldBeTrue();
                }
            }
        }

        #endregion

        private static AwsSignatureHandlerSettings CreateSettings(string serviceName) =>
            new(
                "us-east-1",
                serviceName,
                new ImmutableCredentials(
                    "some access key id",
                    "some secret access key",
                    "some token"));

        private static Task<HttpResponseMessage> InvokeSendAsync(
            AwsSignatureHandler handler,
            HttpRequestMessage request,
            CancellationToken ct) =>
            handler
                .GetType()
                .GetMethod("SendAsync", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(handler, new object[] { request, ct }) as Task<HttpResponseMessage>;

        private static HttpResponseMessage InvokeSend(
            AwsSignatureHandler handler,
            HttpRequestMessage request,
            CancellationToken ct) =>
            handler
                .GetType()
                .GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(handler, new object[] { request, ct }) as HttpResponseMessage;

        private class SinkHandler : HttpMessageHandler
        {
            public HttpRequestMessage Request { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(Send(request, cancellationToken));
            }

            protected override HttpResponseMessage Send(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                Request = request;

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }
}
