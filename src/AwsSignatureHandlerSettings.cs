using System;
using Amazon.Runtime;
using AwsSignatureVersion4.Private;

namespace AwsSignatureVersion4
{
    /// <summary>
    /// Class action as settings for <see cref="AwsSignatureHandler"/>.
    /// </summary>
    public class AwsSignatureHandlerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSignatureHandlerSettings"/> class.
        /// </summary>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        public AwsSignatureHandlerSettings(string regionName, string serviceName, ImmutableCredentials credentials)
        {
            RegionName = regionName ?? throw new ArgumentNullException(nameof(regionName));
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            Credentials = new AWSCredentialsWrapper(credentials ?? throw new ArgumentNullException(nameof(credentials)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSignatureHandlerSettings"/> class.
        /// </summary>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        public AwsSignatureHandlerSettings(string regionName, string serviceName, AWSCredentials credentials)
        {
            RegionName = regionName ?? throw new ArgumentNullException(nameof(regionName));
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSignatureHandlerSettings"/> class
        /// without explicit credentials. Credentials will instead be resolved, just before each
        /// request is signed, using the default AWS credential search order, i.e. the same order
        /// used by <see cref="Amazon.Runtime.Credentials.DefaultAWSCredentialsIdentityResolver"/>,
        /// which includes environment variables, shared AWS credentials/config files, and
        /// container/EC2 instance profile credentials.
        /// </summary>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        public AwsSignatureHandlerSettings(string regionName, string serviceName)
        {
            RegionName = regionName ?? throw new ArgumentNullException(nameof(regionName));
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            Credentials = null;
        }

        /// <summary>
        /// Gets the system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </summary>
        internal string RegionName { get; }

        /// <summary>
        /// Gets the signing name of the service, e.g. "execute-api".
        /// </summary>
        internal string ServiceName { get; }

        /// <summary>
        /// Gets the AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// Null indicates that credentials should be resolved using the default AWS credential
        /// search order.
        /// </summary>
        internal AWSCredentials? Credentials { get; }
    }
}
