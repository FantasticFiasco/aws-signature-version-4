using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace AwsSignatureVersion4.Integration.S3
{
    public class Bucket
    {
        private readonly string name;
        private readonly AmazonS3Client client;
        
        public Bucket(RegionEndpoint region, string name, AWSCredentials credentials)
        {
            this.name = name;
            client = new AmazonS3Client(credentials, region);
        }

        public async Task<BucketObject> PutObjectAsync(string key, string content = "This is some content")
        {
            var request = new PutObjectRequest
            {
                BucketName = name,
                Key = key,
                ContentBody = content
            };

            await client.PutObjectAsync(request);

            return new BucketObject(key, content);
        }
    }
}
