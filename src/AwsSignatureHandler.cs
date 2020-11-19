using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AwsSignatureVersion4
{
    public class AwsSignatureHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }
    }
}
