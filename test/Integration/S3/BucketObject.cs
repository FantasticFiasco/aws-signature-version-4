using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;

namespace AwsSignatureVersion4.Integration.S3
{
    public class BucketObject
    {
        public BucketObject(string key, string content = "This is some content")
        {
            Key = key;
            Content = content;
            StringContent = new StringContent(content);
            MultipartUploadParts = Enumerable.Range(0, 2)
                .Select(_ => new StringContent(RandomNumberGenerator.GetString("abcdefghijklmnopqrstuvwxyz0123456789", 1024 * 1024 * 10))) // 10MB
                .ToArray();
        }

        /// <summary>
        /// Gets the key of the bucket object.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the content of the bucket object.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Gets the HTTP string content representation of the bucket object.
        /// </summary>
        public StringContent StringContent { get; }

        /// <summary>
        /// Gets the parts of the bucket object for multipart upload, each part being 10MB in size.
        /// </summary>
        public StringContent[] MultipartUploadParts { get; }
    }
}
