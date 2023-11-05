using System;
using Amazon.Util;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite;
using AwsSignatureVersion4.TestSuite.Fixtures;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit.Private
{
    public class StringToSignShould : IClassFixture<TestSuiteFixture>
    {
        private readonly Func<string[], Scenario> loadScenario;
        private readonly DateTime utcNow;
        private readonly string region;
        private readonly string serviceName;

        public StringToSignShould(TestSuiteFixture fixture)
        {
            loadScenario = fixture.LoadScenario;
            utcNow = fixture.UtcNow;
            region = fixture.Region.SystemName;
            serviceName = fixture.ServiceName;
        }

        [Theory]
        [InlineData("get-header-key-duplicate")]
        [InlineData("get-header-value-multiline")]
        [InlineData("get-header-value-order")]
        [InlineData("get-header-value-trim")]
        [InlineData("get-unreserved")]
        [InlineData("get-utf8")]
        [InlineData("get-vanilla")]
        [InlineData("get-vanilla-empty-query-key")]
        [InlineData("get-vanilla-query")]
        [InlineData("get-vanilla-query-order-key")]
        [InlineData("get-vanilla-query-order-key-case")]
        [InlineData("get-vanilla-query-order-value")]
        [InlineData("get-vanilla-query-unreserved")]
        [InlineData("get-vanilla-utf8-query")]
        [InlineData("normalize-path", "get-relative")]
        [InlineData("normalize-path", "get-relative-relative")]
        [InlineData("normalize-path", "get-slash")]
        [InlineData("normalize-path", "get-slash-dot-slash")]
        [InlineData("normalize-path", "get-slashes")]
        [InlineData("normalize-path", "get-slash-pointless-dot")]
        [InlineData("normalize-path", "get-space")]
        [InlineData("post-header-key-case")]
        [InlineData("post-header-key-sort")]
        [InlineData("post-header-value-case")]
        [InlineData("post-sts-token", "post-sts-header-after")]
        [InlineData("post-sts-token", "post-sts-header-before")]
        [InlineData("post-vanilla")]
        [InlineData("post-vanilla-empty-query-value")]
        [InlineData("post-vanilla-query")]
        [InlineData("post-x-www-form-urlencoded", Skip = SkipReasons.RedundantContentTypeCharset)]
        [InlineData("post-x-www-form-urlencoded-parameters", Skip = SkipReasons.RedundantContentTypeCharset)]
        public void PassTestSuite(params string[] scenarioName)
        {
            // Arrange
            var scenario = loadScenario(scenarioName);

            // Add header 'X-Amz-Date' since the algorithm at this point expects it on the request
            scenario.Request.AddHeader(HeaderKeys.XAmzDateHeader, utcNow.ToIso8601BasicDateTime());

            // Act
            var (stringToSign, credentialScope) = StringToSign.Build(
                utcNow,
                region,
                serviceName,
                scenario.ExpectedCanonicalRequest);

            // Assert
            stringToSign.ShouldBe(scenario.ExpectedStringToSign);
            credentialScope.ShouldBe(scenario.ExpectedCredentialScope);
        }
    }
}
