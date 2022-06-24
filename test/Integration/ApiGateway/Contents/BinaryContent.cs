using System.IO;
using System.Net.Http;
using System.Text;

namespace AwsSignatureVersion4.Integration.ApiGateway.Contents
{
    public class BinaryContent : IContent
    {
        private readonly byte[] bytes = Encoding.UTF8.GetBytes("some bytes");

        public string AsBase64()
        {
            var base64 = bytes.ToBase64();

            return base64;
        }

        public HttpContent AsHttpContent()
        {
            var stream = new MemoryStream(bytes);
            var content = new StreamContent(stream);

            return content;
        }
    }
}
