using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Shouldly;
using Xunit;

// For more information on S3 multipart uploads, see: https://docs.aws.amazon.com/AmazonS3/latest/userguide/mpuoverview.html

namespace AwsSignatureVersion4.Integration.S3
{
    [Collection("S3")]
    public class PostAsyncShould : S3IntegrationBase
    {
        private static readonly XNamespace ns = "http://s3.amazonaws.com/doc/2006-03-01/";

        public PostAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedMultipartUpload(IamAuthenticationType iamAuthenticationType)
        {
            // Arrange
            var bucketObject = new BucketObject(BucketObjectKey.WithoutPrefix);

            // Act
            // Step 1 - Initiate the upload
            var response = (await HttpClient.PostAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}?uploads",
                null,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType)))
                .EnsureSuccessStatusCode();

            // Extract upload ID from the response
            var uploadId = XDocument.Parse(await response.Content.ReadAsStringAsync())
                .Root
                .Element(ns + "UploadId")
                .Value;

            // Keep track of the uploaded parts, where the key is the part number and the value is the ETag
            var uploadedParts = new List<KeyValuePair<int, string>>();

            // Step 2 - Upload the object parts (part number can be from 1 and 10,000)
            for (var partNumber = 1; partNumber <= bucketObject.MultipartUploadParts.Length; partNumber++)
            {
                response = (await HttpClient.PutAsync(
                    $"{Context.S3BucketUrl}/{bucketObject.Key}?partNumber={partNumber}&uploadId={uploadId}",
                    bucketObject.MultipartUploadParts[partNumber - 1],
                    Context.RegionName,
                    Context.ServiceName,
                    ResolveMutableCredentials(iamAuthenticationType)))
                    .EnsureSuccessStatusCode();

                uploadedParts.Add(new KeyValuePair<int, string>(partNumber, response.Headers.ETag.Tag));
            }

            // Step 3 - Complete the multipart upload
            var content = new StringContent(
                new XDocument(
                    new XElement(ns + "CompleteMultipartUpload",
                        uploadedParts.ConvertAll(part =>
                            new XElement(ns + "Part",
                                new XElement(ns + "PartNumber", part.Key),
                                new XElement(ns + "ETag", part.Value)))))
                .ToString());

            response = (await HttpClient.PostAsync(
                $"{Context.S3BucketUrl}/{bucketObject.Key}?uploadId={uploadId}",
                content,
                Context.RegionName,
                Context.ServiceName,
                ResolveMutableCredentials(iamAuthenticationType)))
                .EnsureSuccessStatusCode();

            // Assert
            XDocument.Parse(await response.Content.ReadAsStringAsync())
                .Root
                .Element(ns + "Key")
                .Value
                .ShouldBe(bucketObject.Key);
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
    }
}
