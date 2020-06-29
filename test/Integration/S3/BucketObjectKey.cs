namespace AwsSignatureVersion4.Integration.S3
{
    public static class BucketObjectKey
    {
        public const string WithoutPrefix = "foo.txt";
        public const string WithSingleLevelPrefix = "foo/bar.txt";
        public const string WithMultiLevelPrefix = "foo/bar/baz.txt";
        public const string WithLowercaseSafeCharacters = "abcdefghijklmnopqrstuvwxyz.txt";
        public const string WithUppercaseSafeCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ.txt";
        public const string WithNumberSafeCharacters = "0123456789.txt";
        public const string WithSpecialSafeCharacters = "!-_.*'().txt";
        public const string WithCharactersThatRequireSpecialHandling = "&$@=;: ,.txt";
        public const string WithUnnormalizedDelimiter = "unnormalized//delimiter.txt";
    }
}
