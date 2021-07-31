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
        /// </summary>
        internal AWSCredentials Credentials { get; }
    }
}
