using System.Net.Http;

namespace AwsSignatureVersion4.Integration.ApiGateway.Contents
{
    public interface IContent
    {
        string AsBase64();

        HttpContent AsHttpContent();
    }
}
