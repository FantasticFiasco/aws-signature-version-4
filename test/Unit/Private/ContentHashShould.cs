using System.Net.Http;
using System.Threading.Tasks;
using AwsSignatureVersion4.Private;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit.Private
{
    public class ContentHashShould
    {
        private const string EmptyContentHash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        [Fact]
        public async Task SupportNullContentAsync()
        {
            // Act
            var actual = await ContentHash.CalculateAsync(null);
            
            // Assert
            actual.ShouldBe(EmptyContentHash);
        }

        [Fact]
        public void SupportNullContent()
        {
            // Act
            var actual = ContentHash.Calculate(null);

            // Assert
            actual.ShouldBe(EmptyContentHash);
        }

        [Fact]
        public async Task SupportEmptyStringContentAsync()
        {
            // Arrange
            HttpContent content = new StringContent(string.Empty);

            // Act
            var actual = await ContentHash.CalculateAsync(content);

            // Assert
            actual.ShouldBe(EmptyContentHash);
        }

        [Fact]
        public void SupportEmptyStringContent()
        {
            // Arrange
            HttpContent content = new StringContent(string.Empty);

            // Act
            var actual = ContentHash.Calculate(content);
            
            // Assert
            actual.ShouldBe(EmptyContentHash);
        }

        [Fact]
        public async Task CalculateValidHashAsync()
        {
            // Arrange
            HttpContent content = new StringContent("foo");

            // Act
            var actual = await ContentHash.CalculateAsync(content);

            // Assert
            actual.Length.ShouldBe(64);
            IsHex(actual).ShouldBeTrue();
        }

        [Fact]
        public void CalculateValidHash()
        {
            // Arrange
            HttpContent content = new StringContent("foo");

            // Act
            var actual = ContentHash.Calculate(content);
            
            // Assert
            actual.Length.ShouldBe(64);
            IsHex(actual).ShouldBeTrue();
        }

        private static bool IsHex(string value)
        {
            foreach (var c in value)
            {
                var success = c >= '0' && c <= '9' ||
                              c >= 'a' && c <= 'f' ||
                              c >= 'A' && c <= 'F';

                if (!success)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
