using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;

namespace AwsSignatureVersion4
{
    public class AwsSignatureHandler : DelegatingHandler
    {
        private readonly AwsSignatureHandlerOptions options;

        public AwsSignatureHandler(AwsSignatureHandlerOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine(request.RequestUri);

            return base.SendAsync(request, cancellationToken);
        }
    }

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
