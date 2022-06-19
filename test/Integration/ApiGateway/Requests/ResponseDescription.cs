using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AwsSignatureVersion4.Integration.ApiGateway.Requests
{
    public class ReceivedRequest
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("queryStringParameters")]
        public Dictionary<string, string[]> QueryStringParameters { get; set; }

        [JsonPropertyName("headers")]
        public Dictionary<string, string[]> Headers { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
