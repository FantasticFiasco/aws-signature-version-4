using System.Net.Http;
using System.Text;

namespace AwsSignatureVersion4.Integration.ApiGateway.Contents
{
    public class EmptyJsonContent : IContent
    {
        public string AsString()
        {
            var json = this.ToJson();

            return json;
        }

        public HttpContent AsHttpContent()
        {
            var json = AsString();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return content;
        }
    }
}
