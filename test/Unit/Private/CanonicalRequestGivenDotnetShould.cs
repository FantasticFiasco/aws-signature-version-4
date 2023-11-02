using System;
using System.Net.Http;
using AwsSignatureVersion4.Private;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit.Private
{
    [Collection("Canonical request - These tests are modifying global scope which prevents them from running in parallel with other canonical request tests")]
    public class CanonicalRequestGivenDotnetShould : IDisposable
    {
        private readonly EnvironmentProbe defaultEnvironmentProbe;

        public CanonicalRequestGivenDotnetShould()
        {
            // Mock .NET environment
            defaultEnvironmentProbe = CanonicalRequest.EnvironmentProbe;
            CanonicalRequest.EnvironmentProbe = new DotnetEnvironmentProbe();
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
        public void IgnoreDuplicateDefaultHeader()
        {
            // Arrange
            var headers = new HttpRequestMessage().Headers;
            headers.Add("some-header-name", "some-header-value");

            var defaultHeaders = new HttpRequestMessage().Headers;
            defaultHeaders.Add("some-header-name", "some-ignored-header-value");

            // Act
            var actual = CanonicalRequest.SortHeaders(headers, defaultHeaders);

            // Assert
            actual["some-header-name"].ShouldBe(new[] { "some-header-value" });
        }

        public void Dispose()
        {
            // Reset environment probe
            CanonicalRequest.EnvironmentProbe = defaultEnvironmentProbe;
        }

        private class DotnetEnvironmentProbe : EnvironmentProbe
        {
            public override bool IsMono => false;
        }
    }
}
