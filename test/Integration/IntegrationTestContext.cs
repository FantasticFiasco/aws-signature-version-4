using Amazon.Runtime;
using AwsSignatureVersion4.TestSuite;

namespace AwsSignatureVersion4.Integration
{
    /// <summary>
    /// Class setting up a context that is valid when we run integration tests towards a real AWS
    /// API Gateway and a real S3 bucket.
    /// </summary>
    public class IntegrationTestContext : Context
    {
        public string RegionName { get; } = Secrets.Aws.Region.SystemName;

        public string ServiceName { get; set; }

        public AWSCredentials UserCredentials { get; } = Secrets.Aws.UserWithPermissions.Credentials;

        public AWSCredentials RoleCredentials { get; } = new AssumeRoleAWSCredentials(
            Secrets.Aws.UserWithoutPermissions.Credentials,
            Secrets.Aws.Role.Arn,
            "signature-version-4-integration-tests");

        public string ApiGatewayUrl { get; } = Secrets.Aws.ApiGateway.Url;

        //public string S3BucketName { get; } = Secrets.Aws.S3.BucketName;

        //public string S3BucketUrl { get; } = $"https://{Secrets.Aws.S3.BucketName}.s3.{Secrets.Aws.Region.SystemName}.amazonaws.com";
    }
}
