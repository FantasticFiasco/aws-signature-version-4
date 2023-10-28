using Xunit;

namespace AwsSignatureVersion4.Integration.S3.Helpers
{
    [CollectionDefinition("S3")]
    public class S3Collection : ICollectionFixture<S3CollectionFixture>
    {
    }
}
