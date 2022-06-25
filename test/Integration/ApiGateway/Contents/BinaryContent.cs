using System.IO;
using System.Net.Http;
using System.Text;

namespace AwsSignatureVersion4.Integration.ApiGateway.Contents
{
    public class BinaryContent : IContent
    {
        private readonly byte[] bytes = Encoding.UTF8.GetBytes("some bytes");

        public string AsString()
        {
            var bytesAsString = Encoding.UTF8.GetString(bytes);

            return bytesAsString;
        }

        public HttpContent AsHttpContent()
        {
            var stream = new MemoryStream(bytes);
            var content = new StreamContent(stream);

            return content;
        }
    }
}
