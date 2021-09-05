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
        public async Task SupportNullContent()
        {
            // Act
            // ReSharper disable once MethodHasAsyncOverload
            var actual1 = ContentHash.Calculate(null);
            var actual2 = await ContentHash.CalculateAsync(null);
            
            // Assert
            actual1.ShouldBe(EmptyContentHash);
            actual2.ShouldBe(EmptyContentHash);
        }

        [Fact]
        public async Task SupportEmptyStringContent()
        {
            // Arrange
            HttpContent content = new StringContent(string.Empty);

            // Act
            // ReSharper disable once MethodHasAsyncOverload
            var actual1 = ContentHash.Calculate(content);
            var actual2 = await ContentHash.CalculateAsync(content);

            // Assert
            actual1.ShouldBe(EmptyContentHash);
            actual2.ShouldBe(EmptyContentHash);
        }

        [Fact]
        public async Task CalculateValidHash()
        {
            // Arrange
            HttpContent content = new StringContent("foo");

            // Act
            // ReSharper disable once MethodHasAsyncOverload
            var actual1 = ContentHash.Calculate(content);
            var actual2 = await ContentHash.CalculateAsync(content);

            // Assert
            actual1.Length.ShouldBe(64);
            actual2.Length.ShouldBe(64);
            IsHex(actual1).ShouldBeTrue();
            IsHex(actual2).ShouldBeTrue();
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
