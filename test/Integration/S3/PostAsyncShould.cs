using System;
using System.Net.Http;
using System.Threading.Tasks;
using AwsSignatureVersion4.Integration.ApiGateway.Authentication;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Integration.S3
{
    [Collection("S3")]
    public class PostAsyncShould : S3IntegrationBase
    {
        public PostAsyncShould(IntegrationTestContext context)
            : base(context)
        {
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task SucceedMultipartUpload(IamAuthenticationType iamAuthenticationType)
        {
            // 1. Initiate the upload
            // 2. Upload the object parts
            //   - Part number between 1 and 10,000
            //   - Record the part number and the ETag value
            // 3. Complete the multipart upload
            //   - Include the upload ID and a list of part numbers and their corresponding ETag values

            // Verification: Assert that all parts have been removed, and the combined object exists.

            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task AbortMultipartUpload(IamAuthenticationType iamAuthenticationType)
        {
            // 1. Initiate the upload
            // 2. Upload some of the object parts
            //   - Part number between 1 and 10,000
            // 3. Abort the multipart upload

            // Verification: Assert that all parts have been removed, and there is no combined object.

            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task ListOngoingMultipartUploads(IamAuthenticationType iamAuthenticationType)
        {
            // 1. Initiate the upload
            // 2. Upload some of the object parts
            //   - Part number between 1 and 10,000
            // 3. List ongoing multipart uploads
            // 4. Abort the multipart upload

            // Verification: Assert that ongoing upload is listed.

            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(IamAuthenticationType.User)]
        [InlineData(IamAuthenticationType.Role)]
        public async Task ListMultipartUploadParts(IamAuthenticationType iamAuthenticationType)
        {
            // 1. Initiate the upload
            // 2. Upload some of the object parts
            //   - Part number between 1 and 10,000
            // 3. List multipart upload parts
            // 4. Abort the multipart upload

            // Verification: Assert that the uploaded parts are listed.

            throw new NotImplementedException();
        }
    }
}
