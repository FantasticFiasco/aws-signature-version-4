using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AwsSignatureVersion4.Integration.ApiGateway.Contents
{
    public static class Extensions
    {
        public static string ToJson<T>(this T self) =>
            JsonConvert.SerializeObject(
                self,
                new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                });

        public static string ToBase64(this string self)
        {
            var bytes = Encoding.UTF8.GetBytes(self);
            var base64 = bytes.ToBase64();

            return base64;
        }

        public static string ToBase64(this byte[] self)
        {
            var base64 = Convert.ToBase64String(self);

            return base64;
        }
    }
}
