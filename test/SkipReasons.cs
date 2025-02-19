namespace AwsSignatureVersion4
{
    public static class SkipReasons
    {
        public const string PlausibleCanonicalUriTestSuiteError =
            "The signing algorithm states the following: 'Each path segment must be URI-encoded " +
            "twice except for Amazon S3 which only gets URI-encoded once.'. This scenario does " +
            "not URL encode the path segments twice, only once.";

        public const string RedundantContentTypeCharset =
            "This scenario is based on the fact that we need to specify the charset in the " +
            "'Content-Type' header, e.g. 'Content-Type:application/x-www-form-urlencoded; " +
            "charset=utf-8'. This is not necessary because .NET will add this encoding if " +
            "omitted by us. We can safely skip this test and rely on integration tests " +
            "where actual content is sent to an AWS API Gateway.";

        public const string RedundantStsTokenScenario =
            "This scenario is based on the fact that the signing algorithm should support STS " +
            "tokens, e.g. by assuming a role. This scenario is already covered by numerous other " +
            "integration tests and can because of this safely be ignored.";

        public const string NotSupportedByApiGateway =
            "This scenario defines a request that isn't supported by AWS API Gateway.";

        public const string NotSupportedByS3 =
            "This scenario defines a request that isn't supported by AWS S3.";
    }
}
