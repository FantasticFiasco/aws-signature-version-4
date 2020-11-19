using System.Net.Http;
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
            var second = new HttpClientHandler();
            var first = new AwsSignatureHandler
            {
                InnerHandler = second
            };

            var client = new HttpClient(first);
            var response = await client.GetStringAsync("https://www.google.com");
            output .WriteLine(response);
        }
    }
}
