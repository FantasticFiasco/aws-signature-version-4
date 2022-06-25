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
