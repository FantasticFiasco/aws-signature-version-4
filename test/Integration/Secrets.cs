using System;
using Amazon;
using Amazon.Runtime;

namespace AwsSignatureVersion4.Integration
{
    public static class Secrets
    {
        public static class Aws
        {
            public static readonly RegionEndpoint Region = RegionEndpoint.GetBySystemName(GetEnvironmentVariable("AWS_REGION"));

            public static class UserWithPermissions
            {
                public static readonly BasicAWSCredentials Credentials = new(
                    GetEnvironmentVariable("AWS_USER_WITH_PERMISSIONS_ACCESS_KEY_ID"),
                    GetEnvironmentVariable("AWS_USER_WITH_PERMISSIONS_SECRET_ACCESS_KEY"));
            }

            public static class UserWithoutPermissions
            {
                public static readonly BasicAWSCredentials Credentials = new(
                    GetEnvironmentVariable("AWS_USER_WITHOUT_PERMISSIONS_ACCESS_KEY_ID"),
                    GetEnvironmentVariable("AWS_USER_WITHOUT_PERMISSIONS_SECRET_ACCESS_KEY"));
            }

            public static class UserWithProvisioningPermissions
            {
                public static readonly BasicAWSCredentials Credentials = new(
                    GetEnvironmentVariable("AWS_USER_WITH_PROVISIONING_PERMISSIONS_ACCESS_KEY_ID"),
                    GetEnvironmentVariable("AWS_USER_WITH_PROVISIONING_PERMISSIONS_SECRET_ACCESS_KEY"));
            }

            public static class Role
            {
                public static readonly string Arn = GetEnvironmentVariable("AWS_ROLE_ARN");
            }

            public static class ApiGateway
            {
                public static readonly string Url = GetEnvironmentVariable("AWS_API_GATEWAY_URL");
            }
        }

        private static string GetEnvironmentVariable(string variable) =>
            Environment.GetEnvironmentVariable(variable) ?? throw new Exception($"Required environment variable '{variable}' is not set.");
    }
}
