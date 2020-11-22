using System;
using Amazon.Runtime;

namespace AwsSignatureVersion4
{
    public class AwsSignatureHandlerOptions
    {
        public AwsSignatureHandlerOptions(string regionName, string serviceName, ImmutableCredentials credentials)
        {
            RegionName = regionName ?? throw new ArgumentNullException(nameof(regionName));
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        }

        public string RegionName { get; }

        public string ServiceName { get; }

        public ImmutableCredentials Credentials { get; }
    }
}
