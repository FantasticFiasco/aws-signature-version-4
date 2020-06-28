using System.Net.Http;

namespace AwsSignatureVersion4.Integration.S3
{
    public class BucketObject
    {
        public BucketObject(string key, StringContent content)
        {
            Key = key;
            Content = content;
        }

        public string Key { get; }

        public StringContent Content { get; }
    }
}
