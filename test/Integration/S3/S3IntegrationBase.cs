namespace AwsSignatureVersion4.Integration.S3
{
    public abstract class S3IntegrationBase : IntegrationBase
    {
        protected S3IntegrationBase(IntegrationTestContext context)
            : base(context)
        {
            context.ServiceName = "s3";
        }
    }
}
