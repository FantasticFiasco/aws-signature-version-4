using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace AwsSignatureVersion4.Integration.S3.Buckets
{
    public class Bucket : IDisposable
    {
        private readonly string name;
        private readonly AmazonS3Client client;

        private Bucket(string name, AmazonS3Client client)
        {
            this.name = name;
            this.client = client;

            Url = $"https://{name}.s3.{client.Config.RegionEndpoint.SystemName}.amazonaws.com";
        }

        public string Url { get; }

        public static async Task<Bucket> CreateAsync(string name, AWSCredentials credentials, RegionEndpoint region)
        {
            var client = new AmazonS3Client(credentials, region);
            var request = new PutBucketRequest
            {
                BucketName = name
            };

            var response = await client.PutBucketAsync(request);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create S3 bucket, got HTTP status code {response.HttpStatusCode}");
            }

            return new Bucket(name, client);
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
            

            return new BucketObject($"https://s3.{client.Config.RegionEndpoint.SystemName}.amazonaws.com/{name}/key", content);
        }

        public async Task DeleteAsync()
        {
            var listObjectsResponse = await client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = name
            });

            if (listObjectsResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to list all object in S3 bucket, got HTTP status code {listObjectsResponse.HttpStatusCode}");
            }

            //foreach (var s3Object in listObjectsResponse.S3Objects)
            //{
            //    var deleteObjectResponse = await client.DeleteObjectAsync(
            //        new DeleteObjectRequest
            //        {
            //            BucketName = name, Key = s3Object.Key
            //        });

            //    if (deleteObjectResponse.HttpStatusCode != HttpStatusCode.OK)
            //    {
            //        throw new Exception($"Unable to delete all object in S3 bucket, got HTTP status code {deleteObjectResponse.HttpStatusCode}");
            //    }
            //}

            var deleteObjectsResponse = await client.DeleteObjectsAsync(
                new DeleteObjectsRequest
                {
                    BucketName = name,
                    Objects = listObjectsResponse.S3Objects.Select(
                        o => new KeyVersion
                        {
                            Key = o.Key
                        }).ToList()
                });

            if (deleteObjectsResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to delete all object in S3 bucket, got HTTP status code {deleteObjectsResponse.HttpStatusCode}");
            }

            var deleteBucketResponse = await client.DeleteBucketAsync(new DeleteBucketRequest
            {
                BucketName = name
            });

            if (deleteBucketResponse.HttpStatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception($"Unable to delete S3 bucket, got HTTP status code {deleteBucketResponse.HttpStatusCode}");
            }
        }

        public void Dispose() => client.Dispose();
    }
}
