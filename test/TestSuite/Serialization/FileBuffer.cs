using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AwsSignatureVersion4.TestSuite.Serialization
{
    public class FileBuffer
    {
        private readonly List<string> rows;

        public FileBuffer(string filePath)
        {
            rows = File.ReadAllLines(filePath).ToList();
        }

        public string this[int index] => rows[index];

        public int Length => rows.Count;

        public string PopFirst() => Pop(0);

        public string TryPopFirst()
        {
            if (rows.Count == 0)
            {
                return null;
            }

            return PopFirst();
        }

        public string PopLast() => Pop(rows.Count - 1);

        public string TryPopLast()
        {
            if (rows.Count == 0)
            {
                return null;
            }

            return PopLast();
        }

        private string Pop(int index)
        {
            var row = rows[index];
            rows.RemoveAt(index);
            return row;
        }

        public string TryPopStartingWith(string prefix)
        {
            for (var index = 0; index < rows.Count; index++)
            {
                if (rows[index].StartsWith(prefix))
                {
                    return Pop(index);
                }
            }

            return null;
        }
    }
}
