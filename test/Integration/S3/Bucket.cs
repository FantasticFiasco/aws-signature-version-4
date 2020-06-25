namespace AwsSignatureVersion4.Integration.S3
{
    /// <summary>
    /// Class representing the structure of objects existing in the S3 bucket after provisioning.
    /// </summary>
    public static class Bucket
    {
        public static class Foo
        {
            public const string Key = "foo.txt";
            public const string Content = "This is foo\n";

            public static class Bar
            {
                public const string Key = "foo/bar.txt";
                public const string Content = "This is bar\n";

                public static class Baz
                {
                    public const string Key = "foo/bar/baz.txt";
                    public const string Content = "This is baz\n";
                }
            }
        }

        public static class SafeCharacters
        {
            public static class Lowercase
            {
                public const string Name = "abcdefghijklmnopqrstuvwxyz.txt";
                public const string Key = "safe-characters/lowercase/" + Name;
                public const string Content = "This file name consists of lowercase characters\n";
            }

            public static class Numbers
            {
                public const string Name = "0123456789.txt";
                public const string Key = "safe-characters/numbers/" + Name;
                public const string Content = "This file name consists of numbers\n";
            }

            public static class SpecialCharacters
            {
                public const string Name = "!-_.'().txt";
                public const string Key = "safe-characters/special-characters/" + Name;
                public const string Content = "This file name consists of special characters\n";
            }

            public static class Uppercase
            {
                public const string Name = "ABCDEFGHIJKLMNOPQRSTUVWXYZ.txt";
                public const string Key = "safe-characters/uppercase/" + Name;
                public const string Content = "This file name consists of uppercase characters\n";
            }
        }
    }
}
