using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AwsSignatureVersion4.Private;
using Shouldly;
using Xunit;

namespace AwsSignatureVersion4.Unit.Private
{
    public class ContentHashShould
    {
        private const string EmptyContentHash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        #region Support null

        [Fact]
        public async Task SupportNullAsync()
        {
            // Act
            var actual = await ContentHash.CalculateAsync(null);
            
            // Assert
            actual.ShouldBe(EmptyContentHash);
        }

        [Fact]
        public void SupportNull()
        {
            // Act
            var actual = ContentHash.Calculate(null);

            // Assert
            actual.ShouldBe(EmptyContentHash);
        }

        #endregion

        #region Support empty string

        [Fact]
        public async Task SupportEmptyStringAsync()
        {
            // Arrange
            using var content = new StringContent(string.Empty);

            // Act
            var actual = await ContentHash.CalculateAsync(content);

            // Assert
            actual.ShouldBe(EmptyContentHash);
        }

        [Fact]
        public void SupportEmptyString()
        {
            // Arrange
            using var content = new StringContent(string.Empty);

            // Act
            var actual = ContentHash.Calculate(content);
            
            // Assert
            actual.ShouldBe(EmptyContentHash);
        }

        #endregion

        #region Support string

        [Fact]
        public async Task SupportStringAsync()
        {
            // Arrange
            using var content = new StringContent("foo");

            // Act
            var actual = await ContentHash.CalculateAsync(content);

            // Assert
            actual.Length.ShouldBe(64);
            IsHex(actual).ShouldBeTrue();
        }

        [Fact]
        public void SupportString()
        {
            // Arrange
            using var content = new StringContent("foo");

            // Act
            var actual = ContentHash.Calculate(content);
            
            // Assert
            actual.Length.ShouldBe(64);
            IsHex(actual).ShouldBeTrue();
        }

        #endregion

        #region Support stream starting from beginning

        [Fact]
        public async Task SupportStreamStartingFromBeginningAsync()
        {
            // Arrange
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("some bytes"));
            using var content = new StreamContent(stream);

            // Act
            var actual = await ContentHash.CalculateAsync(content);

            // Assert
            stream.Position.ShouldBe(0);
            actual.Length.ShouldBe(64);
            IsHex(actual).ShouldBeTrue();
        }

        [Fact]
        public void SupportStreamStartingFromBeginning()
        {
            // Arrange
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("some bytes"));
            using var content = new StreamContent(stream);

            // Act
            var actual = ContentHash.Calculate(content);

            // Assert
            stream.Position.ShouldBe(0);
            actual.Length.ShouldBe(64);
            IsHex(actual).ShouldBeTrue();
        }

        #endregion

        #region Support stream not starting from beginning

        [Fact]
        public async Task SupportStreamNotStartingFromBeginningAsync()
        {
            // Arrange
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("some bytes"));
            stream.Position = 5;

            using var content = new StreamContent(stream);

            // Act
            var actual = await ContentHash.CalculateAsync(content);

            // Assert
            stream.Position.ShouldBe(5);
            actual.Length.ShouldBe(64);
            IsHex(actual).ShouldBeTrue();
        }

        [Fact]
        public void SupportStreamNotStartingFromBeginning()
        {
            // Arrange
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("some bytes"));
            stream.Position = 5;

            using var content = new StreamContent(stream);

            // Act
            var actual = ContentHash.Calculate(content);

            // Assert
            stream.Position.ShouldBe(5);
            actual.Length.ShouldBe(64);
            IsHex(actual).ShouldBeTrue();
        }

        #endregion

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
