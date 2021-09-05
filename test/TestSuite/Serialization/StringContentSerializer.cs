using System.IO;

namespace AwsSignatureVersion4.TestSuite.Serialization
{
    public class StringContentSerializer
    {
        public StringContentSerializer(string filePath)
        {
            FilePath = filePath;
        }

        protected string FilePath { get; }

        public string Deserialize() => File.ReadAllText(FilePath)
            // Make sure we compensate for line endings on Windows
            .Replace("\r\n", "\n");
    }
}
