using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;

namespace AwsSignatureVersion4.Integration.ApiGateway.Contents
{
    public class RichJsonContent : IContent
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = "John";

        [JsonPropertyName("surname")]
        public string Surname { get; set; } = "Doe";

        [JsonPropertyName("age")]
        public int Age { get; set; } = 42;

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
