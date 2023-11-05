using AwsSignatureVersion4.TestSuite.Fixtures;
using Xunit;

namespace AwsSignatureVersion4.Integration.ApiGateway.Fixtures
{
    [CollectionDefinition("API Gateway")]
    public class ApiGatewayCollection : ICollectionFixture<ApiGatewayFixture>, ICollectionFixture<TestSuiteFixture>
    {
    }
}
