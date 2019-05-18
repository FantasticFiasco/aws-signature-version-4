using System;
using Amazon.Runtime;

namespace AWS.SignatureVersion4.TestSuite
{
    /// <summary>
    /// Class setting up a context that is valid when we run tests towards the AWS Test Suite. The
    /// values found in this class can also be found in the test suite.
    /// </summary>
    public class TestSuiteContext : Context
    {
        public string RegionName { get; } = "us-east-1";

        public string ServiceName { get; } = "service";

        public DateTime UtcNow { get; } = new DateTime(
            2015,
            8,
            30,
            12,
            36,
            00,
            DateTimeKind.Utc);

        public ImmutableCredentials Credentials { get; } = new ImmutableCredentials(
            "AKIDEXAMPLE",
            "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY",
            null);
    }
}
