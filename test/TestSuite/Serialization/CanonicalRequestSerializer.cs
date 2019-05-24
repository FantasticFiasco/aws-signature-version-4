using System.IO;

namespace AwsSignatureVersion4.TestSuite.Serialization
{
    public class CanonicalRequestSerializer : StringContentSerializer
    {
        public CanonicalRequestSerializer(string filePath)
            : base(filePath)
        {
        }

        public string DeserializeSignedHeaders()
        {
            var rows = File.ReadAllLines(FilePath);

            // The signed headers are found on the second last row of the canonical request
            return rows[rows.Length - 2];
        }
    }
}
