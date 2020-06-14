namespace AwsSignatureVersion4.Integration.ApiGateway
{
    public abstract class ApiGatewayIntegrationBase : IntegrationBase
    {
        protected ApiGatewayIntegrationBase(IntegrationTestContext context)
            : base(context)
        {
            context.ServiceName = "execute-api";
        }
    }
}
