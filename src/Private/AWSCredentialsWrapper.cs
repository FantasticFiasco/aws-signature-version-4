using System;
using Amazon.Runtime;

namespace AwsSignatureVersion4.Private
{
    /// <summary>
    /// Class wrapping an instance of <see cref="ImmutableCredentials"/> into an
    /// <see cref="AWSCredentials"/>.
    /// </summary>
    public class AWSCredentialsWrapper : AWSCredentials
    {
        private readonly ImmutableCredentials credentials;

        public AWSCredentialsWrapper(ImmutableCredentials credentials)
        {
            this.credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        }

        public override ImmutableCredentials GetCredentials() => credentials;
    }
}
