namespace AwsSignatureVersion4.Integration.S3
{
    public class BucketObject
    {
        public BucketObject(string key, string content)
        {
            Key = key;
            Content = content;
        }

        public string Key { get; }

        public string Content { get; }
    }
}
