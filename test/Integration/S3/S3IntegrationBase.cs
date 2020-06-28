using System;
using Amazon;
using AwsSignatureVersion4.Integration.S3.Helpers;

namespace AwsSignatureVersion4.Integration.S3
{
    public abstract class S3IntegrationBase : IntegrationBase
    {
        private readonly string now;

        protected S3IntegrationBase(IntegrationTestContext context)
            : base(context)
        {
            now = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            context.ServiceName = "s3";

            Bucket = new Bucket(RegionEndpoint.GetBySystemName(context.RegionName), context.S3Url, context.UserCredentials);
        }

        protected Bucket Bucket { get; }

        protected string GenerateRandomTempKey(string namePrefix = null)
        {
            var id = Guid.NewGuid().ToString();

            return $"temp/{namePrefix}{now}-{id}.txt";
        }
    }
}
