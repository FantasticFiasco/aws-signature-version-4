using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amazon.Runtime;
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

            var uploadIds = await ListOngoingAsync(credentials);

            foreach (var uploadId in uploadIds)
            {
                await AbortAsync(
                    BucketObjectKey.WithoutPrefix,
                    uploadId,
                    credentials);
            }
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task Succeed(IamAuthenticationType iamAuthenticationType)
        {
            var bucketObject = new BucketObject(BucketObjectKey.WithoutPrefix);

            // Step 1 - Initiate the upload
            var uploadId = await InitiateAsync(
                bucketObject.Key,
                ResolveMutableCredentials(iamAuthenticationType));

            // Keep track of the uploaded parts, where the key is the part number and the value is the ETag
            var uploadedParts = new List<KeyValuePair<int, string>>();

            // Step 2 - Upload the object parts (part number can be from 1 and 10,000)
            for (var partNumber = 1; partNumber <= bucketObject.MultipartUploadParts.Length; partNumber++)
            {
                var eTag = await UploadPartAsync(
                    bucketObject.Key,
                    uploadId,
                    partNumber,
                    bucketObject.MultipartUploadParts[partNumber - 1],
                    ResolveMutableCredentials(iamAuthenticationType));

                uploadedParts.Add(new KeyValuePair<int, string>(partNumber, eTag));
            }

            // Step 3 - Complete the multipart upload
            var actualKey = await CompleteAsync(
                bucketObject.Key,
                uploadId,
                uploadedParts,
                ResolveMutableCredentials(iamAuthenticationType));

            actualKey.ShouldBe(bucketObject.Key);
        }

        //[Theory]
        //[InlineData(IamAuthenticationType.User)]
        //[InlineData(IamAuthenticationType.Role)]
        //public async Task AbortMultipartUpload(IamAuthenticationType iamAuthenticationType)
        //{
        //    // 1. Initiate the upload
        //    // 2. Upload some of the object parts
        //    //   - Part number between 1 and 10,000
        //    // 3. Abort the multipart upload

        //    // Verification: Assert that all parts have been removed, and there is no combined object.

        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[InlineData(IamAuthenticationType.User)]
        //[InlineData(IamAuthenticationType.Role)]
        //public async Task ListOngoingMultipartUploads(IamAuthenticationType iamAuthenticationType)
        //{
        //    // 1. Initiate the upload
        //    // 2. Upload some of the object parts
        //    //   - Part number between 1 and 10,000
        //    // 3. List ongoing multipart uploads
        //    // 4. Abort the multipart upload

        //    // Verification: Assert that ongoing upload is listed.

        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[InlineData(IamAuthenticationType.User)]
        //[InlineData(IamAuthenticationType.Role)]
        //public async Task ListMultipartUploadParts(IamAuthenticationType iamAuthenticationType)
        //{
        //    // 1. Initiate the upload
        //    // 2. Upload some of the object parts
        //    //   - Part number between 1 and 10,000
        //    // 3. List multipart upload parts
        //    // 4. Abort the multipart upload

        //    // Verification: Assert that the uploaded parts are listed.

        //    throw new NotImplementedException();
        //}

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
            List<KeyValuePair<int, string>> parts,
            AWSCredentials credentials)
        {
            var xml = new XDocument(
                new XElement(ns + "CompleteMultipartUpload",
                    parts.ConvertAll(part =>
                        new XElement(ns + "Part",
                            new XElement(ns + "PartNumber", part.Key),
                            new XElement(ns + "ETag", part.Value)))));

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

        /// <returns>An array with upload IDs.</returns>
        private async Task<string[]> ListOngoingAsync(AWSCredentials credentials)
        {
            var response = await HttpClient.GetAsync(
                $"{Context.S3BucketUrl}/?uploads",
                Context.RegionName,
                Context.ServiceName,
                credentials);

            response.EnsureSuccessStatusCode();

            var document = XDocument.Parse(await response.Content.ReadAsStringAsync());

            return document.Root
                .Descendants(ns + "UploadId")
                .Select(element => element.Value)
                .ToArray();
        }
    }
}
