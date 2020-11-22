using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using AwsSignatureVersion4.Private;

namespace AwsSignatureVersion4
{
    public class AwsSignatureHandler : DelegatingHandler
    {
        private readonly AwsSignatureHandlerOptions options;

        public AwsSignatureHandler(AwsSignatureHandlerOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Signer.SignAsync(
                request,
                null,
                new KeyValuePair<string, IEnumerable<string>>[0],
                DateTime.UtcNow,
                options.RegionName,
                options.ServiceName,
                options.Credentials);

            return await base.SendAsync(request, cancellationToken);
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
