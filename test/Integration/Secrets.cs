using System;

namespace AwsSignatureVersion4.Integration
{
    public static class Secrets
    {
        public static readonly string AwsRegion = GetEnvironmentVariable("AWS_REGION");
        public static readonly string AwsUserWithPermissionsAccessKeyId = GetEnvironmentVariable("AWS_USER_WITH_PERMISSIONS_ACCESS_KEY_ID");
        public static readonly string AwsUserWithPermissionsSecretAccessKey = GetEnvironmentVariable("AWS_USER_WITH_PERMISSIONS_SECRET_ACCESS_KEY");
        public static readonly string AwsUserWithoutPermissionsAccessKeyId = GetEnvironmentVariable("AWS_USER_WITHOUT_PERMISSIONS_ACCESS_KEY_ID");
        public static readonly string AwsUserWithoutPermissionsSecretAccessKey = GetEnvironmentVariable("AWS_USER_WITHOUT_PERMISSIONS_SECRET_ACCESS_KEY");
        public static readonly string AwsRoleArn = GetEnvironmentVariable("AWS_ROLE_ARN");
        public static readonly string AwsApiGatewayUrl = GetEnvironmentVariable("AWS_API_GATEWAY_URL");
        public static readonly string AwsS3BucketName = GetEnvironmentVariable("AWS_S3_BUCKET_NAME");
        public static readonly string AwsS3BucketUrl = GetEnvironmentVariable("AWS_S3_BUCKET_URL");
        
        private static string GetEnvironmentVariable(string variable) =>
            Environment.GetEnvironmentVariable(variable) ?? throw new Exception($"Required environment variable '{variable}' is not set.");
    }
}
