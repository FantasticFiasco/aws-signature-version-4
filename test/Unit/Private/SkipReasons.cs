namespace AWS.SignatureVersion4.Unit.Private
{
    public static class SkipReasons
    {
        public const string UnsupportedHeaders =
            "This scenario depends on the fact that the headers 'Content-Type' and 'Content-Length' " +
            "both should be used in the signing process. This will never be true in .NET since the " +
            "HttpClient is adding these headers based on the attached Content, and in fact throws " +
            "an exception if you try to add them to the HttpRequestMessage. We can safely skip " +
            "these tests and rely on integration tests instead where actual content is sent to an " +
            "API Gateway.";
    }
}
