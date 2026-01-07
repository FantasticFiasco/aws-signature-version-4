using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amazon.Runtime;
using Amazon.SecurityToken.Model;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Shouldly;
using Xunit;

// For more information on S3 multipart uploads, see: https://docs.aws.amazon.com/AmazonS3/latest/userguide/mpuoverview.html

namespace AwsSignatureVersion4.Integration.S3
{
    [Collection("S3")]
    public class MultipartUploadShould : S3IntegrationBase, IAsyncLifetime
    {
        private static readonly XNamespace ns = "http://s3.amazonaws.com/doc/2006-03-01/";

        public MultipartUploadShould(IntegrationTestContext context)
            : base(context)
        {
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Clean up any lingering multipart uploads after each test run.
        /// </summary>
        public async Task DisposeAsync()
        {
            // Use the user credentials to list and abort ongoing multipart uploads. Using the role
            // credentials would have been equally fine, but let's settle on one.
            var credentials = Context.UserCredentials;

            var ongoingUploads = await ListOngoingAsync(credentials);

            foreach (var (Key, UploadId) in ongoingUploads)
            {
                await AbortAsync(Key, UploadId, credentials);
            }
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task Complete(IamAuthenticationType iamAuthenticationType)
        {
            var bucketObject = new BucketObject(BucketObjectKey.WithoutPrefix);

            // Step 1 - Initiate the upload
            var uploadId = await InitiateAsync(
                bucketObject.Key,
                ResolveMutableCredentials(iamAuthenticationType));

            // Keep track of the uploaded parts
            var uploadedParts = new List<(int PartNumber, string ETag)>();

            // Step 2 - Upload the object parts (part number can be from 1 and 10,000)
            for (var partNumber = 1; partNumber <= bucketObject.MultipartUploadParts.Length; partNumber++)
            {
                var eTag = await UploadPartAsync(
                    bucketObject.Key,
                    uploadId,
                    partNumber,
                    bucketObject.MultipartUploadParts[partNumber - 1],
                    ResolveMutableCredentials(iamAuthenticationType));

                uploadedParts.Add((PartNumber: partNumber, ETag: eTag));
            }

            // Step 3 - Complete multipart upload
            var actualKey = await CompleteAsync(
                bucketObject.Key,
                uploadId,
                uploadedParts,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert that the returned object key matches the original object key
            actualKey.ShouldBe(bucketObject.Key);
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task Abort(IamAuthenticationType iamAuthenticationType)
        {
            var bucketObject = new BucketObject(BucketObjectKey.WithoutPrefix);

            // Step 1 - Initiate the upload
            var uploadId = await InitiateAsync(
                bucketObject.Key,
                ResolveMutableCredentials(iamAuthenticationType));

            // Step 2 - Upload a single part (part number can be from 1 and 10,000)
            var partNumber = 1;

            await UploadPartAsync(
                bucketObject.Key,
                uploadId,
                partNumber,
                bucketObject.MultipartUploadParts[partNumber - 1],
                ResolveMutableCredentials(iamAuthenticationType));

            // Step 3 - Abort multipart upload
            await AbortAsync(
                bucketObject.Key,
                uploadId,
                ResolveMutableCredentials(iamAuthenticationType));
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task ListOngoing(IamAuthenticationType iamAuthenticationType)
        {
            var bucketObject = new BucketObject(BucketObjectKey.WithoutPrefix);

            // Step 1 - Initiate the upload
            var uploadId = await InitiateAsync(
                bucketObject.Key,
                ResolveMutableCredentials(iamAuthenticationType));

            // Step 2 - List ongoing multipart uploads
            var ongoingUploads = await ListOngoingAsync(ResolveMutableCredentials(iamAuthenticationType));

            // Assert that the current upload ID is listed
            ongoingUploads.ShouldContain((Key: bucketObject.Key, UploadId: uploadId));
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task ListParts(IamAuthenticationType iamAuthenticationType)
        {
            var bucketObject = new BucketObject(BucketObjectKey.WithoutPrefix);

            // Step 1 - Initiate the upload
            var uploadId = await InitiateAsync(
                bucketObject.Key,
                ResolveMutableCredentials(iamAuthenticationType));

            // Step 2 - Upload a single part (part number can be from 1 and 10,000)
            var partNumber = 1;

            await UploadPartAsync(
                bucketObject.Key,
                uploadId,
                partNumber,
                bucketObject.MultipartUploadParts[partNumber - 1],
                ResolveMutableCredentials(iamAuthenticationType));

            // Step 3 - List parts
            var actualPartNumbers = await ListPartsAsync(
                bucketObject.Key,
                uploadId,
                ResolveMutableCredentials(iamAuthenticationType));

            // Assert that no more or no less than the uploaded part is listed
            actualPartNumbers.ShouldBe([partNumber]);
        }

        /// <returns>The upload ID.</returns>
        private async Task<string> InitiateAsync(string key, AWSCredentials credentials)
        {
            var response = await HttpClient.PostAsync(
                $"{Context.S3BucketUrl}/{key}?uploads",
                null,
                Context.RegionName,
                Context.ServiceName,
                credentials);

            response.EnsureSuccessStatusCode();

            var contentAsString = await response.Content.ReadAsStringAsync();
            var uploadId = XDocument.Parse(contentAsString).Root.Element(ns + "UploadId").Value;

            return uploadId;
        }

        /// <returns>The ETag of the uploaded part.</returns>
        private async Task<string> UploadPartAsync(
            string key,
            string uploadId,
            int partNumber,
            StringContent content,
            AWSCredentials credentials)
        {
            var response = await HttpClient.PutAsync(
                $"{Context.S3BucketUrl}/{key}?uploadId={uploadId}&partNumber={partNumber}",
                content,
                Context.RegionName,
                Context.ServiceName,
                credentials);

            response.EnsureSuccessStatusCode();

            return response.Headers.ETag.Tag;
        }

        /// <returns>The object key.</returns>
        private async Task<string> CompleteAsync(
            string key,
            string uploadId,
            List<(int PartNumber, string ETag)> parts,
            AWSCredentials credentials)
        {
            var xml = new XDocument(
                new XElement(ns + "CompleteMultipartUpload",
                    parts.ConvertAll(part =>
                        new XElement(ns + "Part",
                            new XElement(ns + "PartNumber", part.PartNumber),
                            new XElement(ns + "ETag", part.ETag)))));

            var response = await HttpClient.PostAsync(
                $"{Context.S3BucketUrl}/{key}?uploadId={uploadId}",
                new StringContent(xml.ToString()),
                Context.RegionName,
                Context.ServiceName,
                credentials);

            response.EnsureSuccessStatusCode();

            var document = XDocument.Parse(await response.Content.ReadAsStringAsync());

            return document.Root.Element(ns + "Key").Value;
        }

        private async Task AbortAsync(
            string key,
            string uploadId,
            AWSCredentials credentials)
        {
            var response = await HttpClient.DeleteAsync(
                $"{Context.S3BucketUrl}/{key}?uploadId={uploadId}",
                Context.RegionName,
                Context.ServiceName,
                credentials);

            response.EnsureSuccessStatusCode();
        }

        /// <returns>An array of part IDs</returns>
        private async Task<int[]> ListPartsAsync(
            string key,
            string uploadId,
            AWSCredentials credentials)
        {
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/{key}?uploadId={uploadId}",
                Context.RegionName,
                Context.ServiceName,
                credentials);

            response.EnsureSuccessStatusCode();

            var document = XDocument.Parse(await response.Content.ReadAsStringAsync());

            return document.Root
                .Descendants(ns + "PartNumber")
                .Select(element => int.Parse(element.Value))
                .ToArray();
        }

        private async Task<(string Key, string UploadId)[]> ListOngoingAsync(AWSCredentials credentials)
        {
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/?uploads",
                Context.RegionName,
                Context.ServiceName,
                credentials);

            response.EnsureSuccessStatusCode();

            var document = XDocument.Parse(await response.Content.ReadAsStringAsync());

            return document.Root
                .Descendants(ns + "Upload")
                .Select(upload =>
                {
                    return (
                        Key: upload.Element(ns + "Key").Value,
                        UploadId: upload.Element(ns + "UploadId").Value);
                })
                .ToArray();
        }
    }
}
