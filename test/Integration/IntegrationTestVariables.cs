using System;
using System.Collections.Generic;
using System.IO;

namespace AWS.SignatureVersion4.Integration
{
    public class IntegrationTestVariables
    {
        private const string FileName = "local-variables.txt";

        private readonly IDictionary<string, string> variables;

        public IntegrationTestVariables()
        {
            variables = new Dictionary<string, string>();
        }

        public void Load()
        {
            if (!File.Exists(FileName)) return;

            foreach (var line in File.ReadAllLines(FileName))
            {
                var parts = line.Split('=');
                variables.Add(parts[0], parts[1]);
            }
        }

        public string GetValue(string variableName) =>
            variables.ContainsKey(variableName)
                ? variables[variableName]
                : Environment.GetEnvironmentVariable(variableName);
    }
}
