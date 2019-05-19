using System;

namespace AWS.SignatureVersion4.Integration
{
    public static class IntegrationTestEnvironmentVariables
    {
        public static readonly string AwsRegion = Environment.GetEnvironmentVariable("AWS_REGION");

        public static readonly string AwsUserAccessKeyId = Environment.GetEnvironmentVariable("AWS_USER_ACCESS_KEY_ID");

        public static readonly string AwsUserSecretAccessKey = Environment.GetEnvironmentVariable("AWS_USER_SECRET_ACCESS_KEY");

        public static readonly string AwsRoleAccessKeyId = Environment.GetEnvironmentVariable("AWS_ROLE_ACCESS_KEY_ID");

        public static readonly string AwsRoleSecretAccessKey = Environment.GetEnvironmentVariable("AWS_ROLE_SECRET_ACCESS_KEY");

        public static readonly string AwsApiGatewayUrl = Environment.GetEnvironmentVariable("AWS_API_GATEWAY_URL");
    }
}
