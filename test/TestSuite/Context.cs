using System.IO;

namespace AwsSignatureVersion4.TestSuite
{
    public abstract class Context
    {
        public Scenario LoadScenario(params string[] scenarioName)
        {
            var scenarioPath = Path.Combine(
                "..",
                "..",
                "..",
                "TestSuite",
                "Blueprint",
                "aws-sig-v4-test-suite",
                Path.Combine(scenarioName));

            return new Scenario(scenarioPath);
        }
    }
}
