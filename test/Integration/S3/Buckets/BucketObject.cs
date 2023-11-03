using System.Net.Http;

namespace AwsSignatureVersion4.Integration.S3.Buckets
{
    public class BucketObject
    {
        public BucketObject(string url, string content = "This is some content")
        {
            Url = url;
            Content = content;
            StringContent = new StringContent(content);
        }

        public string Url { get; }
        
        public string Content { get; }

        public StringContent StringContent { get; }
    }
}
