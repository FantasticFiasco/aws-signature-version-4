namespace AWS.SignatureVersion4
{
    public static class SkipReasons
    {
        public const string RedundantStsTokenScenario =
            "This scenario is based on the fact that the signing algorithm should support STS " +
            "tokens, e.g. by assuming a role. This scenario is already covered by numerous other " +
            "integration tests and can because of this safely be ignored.";

        public const string UnsupportedHeaders =
            "This scenario depends on the fact that the headers 'Content-Type' and 'Content-Length' " +
            "both should be used in the signing process. This will never be true in .NET since the " +
            "HttpClient is adding these headers based on the attached Content, and in fact throws " +
            "an exception if you try to add them to the HttpRequestMessage. We can safely skip " +
            "these tests and rely on integration tests instead where actual content is sent to an " +
            "API Gateway.";
    }
}
