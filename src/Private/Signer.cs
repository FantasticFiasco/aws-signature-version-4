using System;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Util;

namespace AwsSignatureVersion4.Private
{
    public static class Signer
    {
        public static async Task<Result> SignAsync(
            HttpClient httpClient,
            HttpRequestMessage request,
            DateTime now,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Headers.Contains(HeaderKeys.XAmzDateHeader)) throw new ArgumentException(ErrorMessages.XAmzDateHeaderExists, nameof(request));
            if (request.Headers.Authorization != null) throw new ArgumentException(ErrorMessages.AuthorizationHeaderExists, nameof(request));
            if (request.Headers.Contains(HeaderKeys.AuthorizationHeader)) throw new ArgumentException(ErrorMessages.AuthorizationHeaderExists, nameof(request));
            if (regionName == null) throw new ArgumentNullException(nameof(regionName));
            if (serviceName == null) throw new ArgumentNullException(nameof(serviceName));
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            UpdateRequestUri(httpClient, request);

            var contentHash = await ContentHash.BuildAsync(request.Content);

            // Add required headers
            request.AddHeader(HeaderKeys.XAmzDateHeader, now.ToIso8601BasicDateTime());

            // Add conditional headers
            request.AddHeaderIf(credentials.UseToken, HeaderKeys.XAmzSecurityTokenHeader, credentials.Token);
            request.AddHeaderIf(!request.Headers.Contains(HeaderKeys.HostHeader), HeaderKeys.HostHeader, request.RequestUri.Host);
            request.AddHeaderIf(serviceName == ServiceName.S3, HeaderKeys.XAmzContentSha256Header, contentHash);

            // Build the canonical request
            var (canonicalRequest, signedHeaders) = CanonicalRequest.Build(request, httpClient.DefaultRequestHeaders, contentHash);

            // Build the string to sign
            var (stringToSign, credentialScope) = StringToSign.Build(
                    now,
                    regionName,
                    serviceName,
                    canonicalRequest);

            // Build the authorization header
            var authorizationHeader = AuthorizationHeader.Build(
                    now,
                    regionName,
                    serviceName,
                    credentials,
                    signedHeaders,
                    credentialScope,
                    stringToSign);

            // Add the authorization header
            request.Headers.TryAddWithoutValidation(HeaderKeys.AuthorizationHeader, authorizationHeader);

            return new Result(canonicalRequest, stringToSign, authorizationHeader);
        }

        private static void UpdateRequestUri(HttpClient httpClient, HttpRequestMessage request)
        {
            if (request.RequestUri == null && httpClient.BaseAddress == null) throw new InvalidOperationException(ErrorMessages.InvalidRequestUri);

            Uri requestUri = null;

            if (request.RequestUri == null)
            {
                requestUri = httpClient.BaseAddress;
            }
            else
            {
                // If the request Uri is an absolute Uri, just use it. Otherwise try to combine it
                // with the base Uri.
                if (!request.RequestUri.IsAbsoluteUri)
                {
                    if (httpClient.BaseAddress == null) throw new InvalidOperationException(ErrorMessages.InvalidRequestUri);

                    requestUri = new Uri(httpClient.BaseAddress, request.RequestUri);
                }
            }

            // We modified the original request Uri. Assign the new Uri to the request message.
            if (requestUri != null)
            {
                request.RequestUri = requestUri;
            }
        }
    }
}
