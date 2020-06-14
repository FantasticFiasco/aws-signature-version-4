namespace AwsSignatureVersion4.Integration.S3
{
    /// <summary>
    /// Class representing the structure of items existing in the S3 bucket after provisioning.
    /// </summary>
    public class Bucket
    {
        public static class Foo
        {
            public const string Key = "foo.txt";
        }

        public static class Bar
        {
            public static class Baz
            {
                public const string Key = "foo/bar.txt";
            }
        }
    }
}
