using System.Net.Http;
using System.Text;

namespace AwsSignatureVersion4.Integration.ApiGateway.Contents
{
    public class EmptyJsonContent : IContent
    {
        public string AsBase64()
        {
            var json = this.ToJson();
            var base64 = json.ToBase64();

            return base64;
        }

        public HttpContent AsHttpContent()
        {
            var json = this.ToJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return content;
        }
    }
}
