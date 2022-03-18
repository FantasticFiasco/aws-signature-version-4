using System;
using System.Net.Http;
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

        public static StringContent ToJsonContent(this Type self) =>
            new(
                Activator.CreateInstance(self).ToJson(),
                Encoding.UTF8,
                "application/json");
    }
}
