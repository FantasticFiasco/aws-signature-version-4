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
    }
}
