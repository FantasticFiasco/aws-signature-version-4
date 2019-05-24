using System.IO;

namespace AwsSignatureVersion4.TestSuite.Serialization
{
    public class StringToSignSerializer : StringContentSerializer
    {
        public StringToSignSerializer(string filePath)
            : base(filePath)
        {
        }

        public string DeserializeCredentialScope()
        {
            var rows = File.ReadAllLines(FilePath);

            // The credential scope is found on the second last row of the signed headers
            return rows[rows.Length - 2];
        }
    }
}
