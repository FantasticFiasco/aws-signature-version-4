using System.Net.Http;

namespace AwsSignatureVersion4.Integration.ApiGateway.Contents
{
    public interface IContent
    {
        string AsString();

        HttpContent AsHttpContent();
    }
}
