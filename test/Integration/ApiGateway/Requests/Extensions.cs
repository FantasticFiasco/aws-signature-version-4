using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AwsSignatureVersion4.Integration.ApiGateway.Requests
{
    public static class Extensions
    {
        public static async Task<ReceivedRequest> ReadReceivedRequestAsync(this HttpContent self)
        {
            var content = await self.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReceivedRequest>(content);
        }
    }
}
