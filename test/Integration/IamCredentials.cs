using System;
using Amazon.Runtime;

namespace AwsSignatureVersion4.Integration
{
    public class IamCredentials
    {
        private readonly AWSCredentials user;
        private readonly AWSCredentials role;

        public IamCredentials()
        {
            user = Secrets.Aws.UserWithPermissions.Credentials;
            role = new AssumeRoleAWSCredentials(
                Secrets.Aws.UserWithoutPermissions.Credentials,
                Secrets.Aws.Role.Arn,
                "signature-version-4-integration-tests");
        }

        public AWSCredentials ResolveMutableCredentials(IamAuthenticationType type)
        {
            return type switch
            {
                IamAuthenticationType.User => user,
                IamAuthenticationType.Role => role,
                _ => throw new NotImplementedException($"The authentication type {type} is not implemented")
            };
        }

        public ImmutableCredentials ResolveImmutableCredentials(IamAuthenticationType type)
        {
            return ResolveMutableCredentials(type).GetCredentials();
        }
    }
}
