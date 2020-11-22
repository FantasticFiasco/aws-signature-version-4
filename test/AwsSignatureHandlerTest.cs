using System;
using System.Net.Http;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AwsSignatureVersion4
{
    public class AwsSignatureHandlerTest
    {
        private readonly ITestOutputHelper output;

        public AwsSignatureHandlerTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async void Test()
        {
            //var second = new HttpClientHandler();
            //var first = new AwsSignatureHandler
            //{
            //    InnerHandler = second
            //};

            //var client = new HttpClient(first);

            var services = new ServiceCollection();
            services.AddTransient<AwsSignatureHandler>();
            services.AddTransient<AwsSignatureHandlerOptions>(provider => new AwsSignatureHandlerOptions("", "", new ImmutableCredentials("a", "b", "c")));

            services.AddHttpClient("test",
                    c =>
                    {
                        c.BaseAddress = new Uri("https://www.google.com");
                        c.DefaultRequestHeaders.Add("SOME-HEADER", "value");
                    })
                .AddHttpMessageHandler<AwsSignatureHandler>();

            var client = services
                .BuildServiceProvider()
                .GetService<IHttpClientFactory>()
                .CreateClient("test");

            var response = await client.GetStringAsync("/");
            output.WriteLine(response);
        }
    }
}
