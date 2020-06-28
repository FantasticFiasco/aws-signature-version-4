using System.Net.Http;

namespace AwsSignatureVersion4.Integration.S3
{
    public class BucketObject
    {
        public BucketObject(string key, string content = "This is some content")
        {
            Key = key;
            Content = new StringContent(content);
        }

        public string Key { get; }

        public StringContent Content { get; }
    }
}
