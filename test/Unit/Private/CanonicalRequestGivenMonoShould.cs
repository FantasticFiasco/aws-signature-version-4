using System;
using System.Net.Http;
using AwsSignatureVersion4.Private;
using AwsSignatureVersion4.TestSuite;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit.Private
{
    public class CanonicalRequestGivenMonoShould : IClassFixture<TestSuiteContext>, IDisposable
    {
        private readonly TestSuiteContext context;
        private readonly EnvironmentProbe defaultEnvironmentProbe;

        public CanonicalRequestGivenMonoShould(TestSuiteContext context)
        {
            this.context = context;

            context.AdjustHeaderValueSeparator();

            // Mock Mono environment
            defaultEnvironmentProbe = CanonicalRequest.EnvironmentProbe;
            CanonicalRequest.EnvironmentProbe = new MonoEnvironmentProbe();
        }

        [Fact]
        public void RespectDefaultHeader()
        {
            // Arrange
            var headers = new HttpRequestMessage().Headers;

            var defaultHeaders = new HttpRequestMessage().Headers;
            defaultHeaders.Add("some-header-name", "some-header-value");

            // Act
            var actual = CanonicalRequest.SortHeaders(headers, defaultHeaders);

            // Assert
            actual["some-header-name"].ShouldBe(new[] { "some-header-value" });
        }

        [Fact]
        public void RespectDuplicateDefaultHeader()
        {
            // Arrange
            var headers = new HttpRequestMessage().Headers;
            headers.Add("some-header-name", "some-header-value");

            var defaultHeaders = new HttpRequestMessage().Headers;
            defaultHeaders.Add("some-header-name", "some-other-header-value");

            // Act
            var actual = CanonicalRequest.SortHeaders(headers, defaultHeaders);

            // Assert
            actual["some-header-name"].ShouldBe(new[] { "some-header-value", "some-other-header-value" });
        }

        public void Dispose()
        {
            // Reset header value separator
            context.ResetHeaderValueSeparator();

            // Reset environment probe
            CanonicalRequest.EnvironmentProbe = defaultEnvironmentProbe;
        }

        private class MonoEnvironmentProbe : EnvironmentProbe
        {
            public override bool IsMono { get; } = true;
        }
    }
}
