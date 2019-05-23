using System;
using System.Collections.Generic;
using System.IO;

namespace AWS.SignatureVersion4.Integration
{
    /// <summary>
    /// The integration test values are not static but is retrieved from either environment or from
    /// a local file called "local-integration-test-variables.txt" in the output directory.
    /// </summary>
    public class IntegrationTestVariables
    {
        private const string FileName = "local-integration-test-variables.txt";

        private readonly Lazy<IDictionary<string, string>> variables;

        public IntegrationTestVariables()
        {
            variables = new Lazy<IDictionary<string, string>>(LoadVariables);
        }

        public string GetValue(string variableName) =>
            variables.Value.ContainsKey(variableName)
                ? variables.Value[variableName]
                : Environment.GetEnvironmentVariable(variableName);

        private static IDictionary<string, string> LoadVariables()
        {
            var variables = new Dictionary<string, string>();

            if (File.Exists(FileName))
            {
                foreach (var line in File.ReadAllLines(FileName))
                {
                    if (line == string.Empty) continue;

                    var parts = line.Split('=');
                    variables.Add(parts[0], parts[1]);
                }
            }

            return variables;
        }
    }
}
