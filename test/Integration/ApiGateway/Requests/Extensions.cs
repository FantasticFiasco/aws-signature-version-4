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
            return content.DeserializeReceivedRequest();
        }

        public static ReceivedRequest DeserializeReceivedRequest(this string self) =>
            JsonConvert.DeserializeObject<ReceivedRequest>(self);
    }
}
