using System;
using Amazon.Runtime;
using AwsSignatureVersion4.Private;

namespace AwsSignatureVersion4.TestSuite
{
    /// <summary>
    /// Class setting up a context that is valid when we run tests towards the AWS Test Suite. The
    /// values found in this class can also be found in the test suite.
    /// </summary>
    public class TestSuiteContext : Context
    {
        private string defaultHeaderValueSeparator;

        public string RegionName { get; } = "us-east-1";

        public string ServiceName { get; } = "service";

        public DateTime UtcNow { get; } = new(
            2015,
            8,
            30,
            12,
            36,
            00,
            DateTimeKind.Utc);

        public ImmutableCredentials Credentials { get; } = new(
            "AKIDEXAMPLE",
            "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY",
            null);

        /// <summary>
        /// The header value separator chosen by Microsoft in .NET is ", " and not "," as defined
        /// by the test suite. This means that we have to change the default behavior to match the
        /// test suite.
        /// </summary>
        public void AdjustHeaderValueSeparator()
        {
            defaultHeaderValueSeparator = CanonicalRequest.HeaderValueSeparator;
            CanonicalRequest.HeaderValueSeparator = ",";
        }

        /// <summary>
        /// Lets reset the default header value separator before we continue with the rest of the
        /// tests.
        /// </summary>
        public void ResetHeaderValueSeparator() => CanonicalRequest.HeaderValueSeparator = defaultHeaderValueSeparator;
    }
}
