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

        public string Deserialize() => File.ReadAllText(FilePath);
    }
}
