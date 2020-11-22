using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit
{
    public class AwsSignatureHandlerShould
    {
        private readonly SinkHandler sinkHandler;
        private readonly AwsSignatureHandler handler;
        
        public AwsSignatureHandlerShould()
        {
            sinkHandler = new SinkHandler();

            var settings = new AwsSignatureHandlerSettings(
                "us-east-1",
                "execute-api",
                new ImmutableCredentials(
                    "some access key id",
                    "some secret access key",
                    "some token"));

            handler = new AwsSignatureHandler(settings)
            {
                InnerHandler = sinkHandler
            };
        }


        [Fact]
        public void BeIdempotent()
        {
            // Arrange
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri("https://github.com/FantasticFiasco/aws-signature-version-4"));

            var ct = new CancellationToken();

            var method = handler.GetType().GetMethod("SendAsync", BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            for (int i = 0; i < 2; i++)
            {
                var response = method.Invoke(handler, new object[] { request, ct }) as Task<HttpResponseMessage>;

                // Assert
                response.Status.ShouldBe(TaskStatus.RanToCompletion);
            }
        }

        class SinkHandler : HttpMessageHandler
        {
            public HttpRequestMessage ReceivedMessage { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                ReceivedMessage = request;

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
        }
    }
}
