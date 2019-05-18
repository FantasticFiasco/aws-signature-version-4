using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Amazon.Runtime.Internal.Auth;
using Amazon.Util;

namespace AWS.SignatureVersion4.Private
{
    /// <summary>
    /// Building the authorization header is one of the steps in the Signature Version 4 process. For
    /// more information, please read
    /// https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html.
    /// </summary>
    public static class CanonicalRequest
    {
        /// <returns>
        /// The first value is the canonical request, the second value is the signed headers.
        /// </returns>
        public static async Task<(string, string)> BuildAsync(HttpRequestMessage request)
        {
            var builder = new StringBuilder();

            // The HTTP request method (GET, PUT, POST, etc.), followed by a newline character
            builder.Append($"{request.Method}\n");

            // Add the canonical URI parameter, followed by a newline character. The canonical URI
            // is the URI-encoded version of the absolute path component of the URI, which is
            // everything in the URI from the HTTP host to the question mark character ("?") that
            // begins the query string parameters (if any).
            //
            // Normalize URI paths according to <see href="https://tools.ietf.org/html/rfc3986">RFC
            // 3986</see>. Remove redundant and relative path components. Each path segment must be
            // URI-encoded twice (
            // <see href="https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-query-string-auth.html">
            // except for Amazon S3 which only gets URI-encoded once</see>).
            var pathSegments = request.RequestUri.LocalPath
                .Replace("//", "/")
                .Split('/')
                .Select(Uri.EscapeUriString);

            builder.Append($"{string.Join("/", pathSegments)}\n");

            // Add the canonical query string, followed by a newline character. If the request does
            // not include a query string, use an empty string (essentially, a blank line).
            //
            // To construct the canonical query string, complete the following steps:
            //
            // a. Sort the parameter names by character code point in ascending order. Parameters
            //    with duplicate names should be sorted by value. For example, a parameter name
            //    that begins with the uppercase letter F precedes a parameter name that begins
            //    with a lowercase letter b.
            // b. URI-encode each parameter name and value according to the following rules:
            //    - Do not URI-encode any of the unreserved characters that RFC 3986 defines: A-Z,
            //      a-z, 0-9, hyphen ( - ), underscore ( _ ), period ( . ), and tilde ( ~ ).
            //    - Percent-encode all other characters with %XY, where X and Y are hexadecimal
            //      characters (0-9 and uppercase A-F). For example, the space character must be
            //      encoded as %20 (not using '+', as some encoding schemes do) and extended UTF-8
            //      characters must be in the form %XY%ZA%BC.
            // c. Build the canonical query string by starting with the first parameter name in the
            //    sorted list.
            // d. For each parameter, append the URI-encoded parameter name, followed by the equals
            //    sign character (=), followed by the URI-encoded parameter value. Use an empty
            //    string for parameters that have no value.
            // e. Append the ampersand character (&) after each parameter value, except for the
            //    last value in the list.
            var parameters = SortQueryParameters(request.RequestUri.Query)
                .SelectMany(
                    parameter => parameter.Value.Select(
                        parameterValue => $"{Uri.EscapeUriString(parameter.Key)}={Uri.EscapeUriString(parameterValue)}"));

            builder.Append($"{string.Join("&", parameters)}\n");

            // Add the canonical headers, followed by a newline character. The canonical headers
            // consist of a list of all the HTTP headers that you are including with the signed
            // request.
            //
            // To create the canonical headers list, convert all header names to lowercase and
            // remove leading spaces and trailing spaces. Convert sequential spaces in the header
            // value to a single space.
            //
            // Build the canonical headers list by sorting the (lowercase) headers by character
            // code and then iterating through the header names. Construct each header according to
            // the following rules:
            //
            // - Append the lowercase header name followed by a colon.
            // - Append a comma-separated list of values for that header. Do not sort the values in
            //   headers that have multiple values.
            // - Append a new line ('\n').
            var sortedHeaders = SortHeaders(request.Headers);

            foreach (var header in sortedHeaders)
            {
                builder.Append($"{header.Key}:{string.Join(",", header.Value)}\n");
            }

            builder.Append('\n');

            // Add the signed headers, followed by a newline character. This value is the list of
            // headers that you included in the canonical headers. By adding this list of headers,
            // you tell AWS which headers in the request are part of the signing process and which
            // ones AWS can ignore (for example, any additional headers added by a proxy) for
            // purposes of validating the request.
            //
            // To create the signed headers list, convert all header names to lowercase, sort them
            // by character code, and use a semicolon to separate the header names.
            //
            // Build the signed headers list by iterating through the collection of header names,
            // sorted by lowercase character code. For each header name except the last, append a
            // semicolon (';') to the header name to separate it from the following header name.
            var signedHeaders = string.Join(";", sortedHeaders.Keys);
            builder.Append($"{signedHeaders}\n");

            // Use a hash (digest) function like SHA256 to create a hashed value from the payload
            // in the body of the HTTP or HTTPS request.
            //
            // If the payload is empty, use an empty string as the input to the hash function.
            var requestPayload = request.Content != null
                ? await request.Content.ReadAsByteArrayAsync()
                : new byte[0];

            var hash = AWS4Signer.ComputeHash(requestPayload);
            var hex = AWSSDKUtils.ToHex(hash, true);

            builder.Append(hex);

            return (builder.ToString(), signedHeaders);
        }

        public static SortedDictionary<string, List<string>> SortHeaders(HttpRequestHeaders headers)
        {
            var sortedHeaders = new SortedDictionary<string, List<string>>(StringComparer.Ordinal);

            foreach (var header in headers)
            {
                // Convert header name to lowercase
                var headerName = header.Key.ToLowerInvariant();

                // Create header if it doesn't already exist
                if (!sortedHeaders.TryGetValue(headerName, out var headerValues))
                {
                    headerValues = new List<string>();
                    sortedHeaders.Add(headerName, headerValues);
                }

                // Remove leading and trailing header value spaces, and convert sequential spaces
                // into a single space
                headerValues.AddRange(header.Value.Select(headerValue => headerValue.Trim().NormalizeWhiteSpace()));
            }

            return sortedHeaders;
        }

        public static SortedList<string, List<string>> SortQueryParameters(string query)
        {
            var sortedQueryParameters = new SortedList<string, List<string>>(StringComparer.Ordinal);
            var queryParameters = HttpUtility.ParseQueryString(query);

            foreach (string parameterName in queryParameters)
            {
                // Create query parameter if it doesn't already exist
                if (!sortedQueryParameters.TryGetValue(parameterName, out var parameterValues))
                {
                    parameterValues = new List<string>();
                    sortedQueryParameters.Add(parameterName, parameterValues);
                }

                parameterValues.AddRange(queryParameters.GetValues(parameterName));
            }

            // Sort the query parameter values
            foreach (var queryParameter in sortedQueryParameters)
            {
                queryParameter.Value.Sort(StringComparer.Ordinal);
            }

            return sortedQueryParameters;
        }
    }
}
