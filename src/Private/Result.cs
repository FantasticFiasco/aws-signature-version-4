namespace AwsSignatureVersion4.Private
{
    public class Result
    {
        public Result(
            string canonicalRequest,
            string stringToSign,
            string authorizationHeader)
        {
            CanonicalRequest = canonicalRequest;
            StringToSign = stringToSign;
            AuthorizationHeader = authorizationHeader;
        }

        public string CanonicalRequest { get; }

        public string StringToSign { get; }

        public string AuthorizationHeader { get; }
    }
}
