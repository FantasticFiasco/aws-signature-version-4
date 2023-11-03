using Xunit;

namespace AwsSignatureVersion4.Integration.S3.Fixtures
{
    [CollectionDefinition("S3")]
    public class S3Collection : ICollectionFixture<S3Fixture>
    {
    }
}
