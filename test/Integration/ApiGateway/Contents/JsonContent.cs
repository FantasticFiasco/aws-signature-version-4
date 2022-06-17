using System.Text.Json.Serialization;

namespace AwsSignatureVersion4.Integration.ApiGateway.Contents
{
    public class JsonContent
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = "John";

        [JsonPropertyName("surname")]
        public string Surname { get; set; } = "Doe";

        [JsonPropertyName("age")]
        public int Age { get; set; } = 42;
    }
}
