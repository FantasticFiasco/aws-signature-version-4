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
        public string RegionName { get; } = Secrets.AwsRegion;

        public string ServiceName { get; set; }

        public AWSCredentials UserCredentials { get; } = new BasicAWSCredentials(
            Secrets.AwsUserWithPermissionsAccessKeyId,
            Secrets.AwsUserWithPermissionsSecretAccessKey);

        public AWSCredentials RoleCredentials { get; } = new AssumeRoleAWSCredentials(
            new BasicAWSCredentials(
                Secrets.AwsUserWithoutPermissionsAccessKeyId,
                Secrets.AwsUserWithoutPermissionsSecretAccessKey),
            Secrets.AwsRoleArn,
            "signature-version-4-integration-tests");

        public string ApiGatewayUrl { get; } = Secrets.AwsApiGatewayUrl;

        public string S3BucketName { get; } = Secrets.AwsS3BucketName;

        public string S3BucketUrl { get; } = Secrets.AwsS3BucketUrl;
    }
}
