﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Amazon.Util;

namespace AwsSignatureVersion4.Private
{
    /// <summary>
    /// Building the authorization header is one of the steps in the Signature Version 4 process. For
    /// more information, please read
    /// https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html.
    /// </summary>
    public static class CanonicalRequest
    {
        /// <summary>
        /// Gets or sets an instance capable of probing the environment.
        /// </summary>
        public static EnvironmentProbe EnvironmentProbe { get; set; } = new();

        /// <summary>
        /// Gets or sets the header value separator. The default value is ", " and it is defined in
        /// <see href="https://github.com/dotnet/corefx/blob/master/src/System.Net.Http/src/System/Net/Http/Headers/HttpHeaderParser.cs">
        /// HttpHeaderParser</see> in the .NET source code. It is used when serializing a header
        /// with multiple values into a HTTP request. For some other languages this separator is
        /// plainly ",", but Microsoft has chosen to go with ", ".
        /// </summary>
        public static string HeaderValueSeparator { get; set; } = ", ";

        /// <returns>
        /// The first value is the canonical request, the second value is the signed headers.
        /// </returns>
        public static (string, string) Build(
            string serviceName,
            HttpRequestMessage request,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> defaultHeaders,
            string contentHash)
        {
            if (request.RequestUri == null) throw new InvalidOperationException(ErrorMessages.InvalidRequestUri);

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
            var canonicalResourcePath = GetCanonicalResourcePath(serviceName, request.RequestUri);

            builder.Append($"{canonicalResourcePath}\n");

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
                        parameterValue => $"{AWSSDKUtils.UrlEncode(parameter.Key, false)}={AWSSDKUtils.UrlEncode(parameterValue, false)}"));

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
            //   PLEASE NOTE: Microsoft has chosen to separate the header values with ", ", not ","
            //   as defined by the Canonical Request algorithm.
            // - Append a new line ('\n').
            var sortedHeaders = SortHeaders(request.Headers, defaultHeaders);

            foreach (var header in sortedHeaders)
            {
                // The 'user-agent' header should be treated differently, as discovered in
                // https://github.com/FantasticFiasco/aws-signature-version-4/issues/1155
                var separator = header.Key.ToLowerInvariant() == "user-agent" ? " " : HeaderValueSeparator;
                builder.Append($"{header.Key}:{string.Join(separator, header.Value)}\n");
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
            builder.Append(contentHash);

            return (builder.ToString(), signedHeaders);
        }

        public static string GetCanonicalResourcePath(string serviceName, Uri requestUri)
        {
            var path = serviceName == ServiceName.S3
                ? requestUri.LocalPath
                : requestUri.AbsolutePath.Replace("//", "/");

            var pathSegments = path
                .Split('/')
                .Select(pathSegment => AWSSDKUtils.UrlEncode(pathSegment, false));

            return string.Join("/", pathSegments);
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

                var queryParameterValues = queryParameters.GetValues(parameterName);
                if (queryParameterValues?.Length > 0)
                {
                    parameterValues.AddRange(queryParameterValues);
                }
            }

            // Sort the query parameter values
            foreach (var queryParameter in sortedQueryParameters)
            {
                queryParameter.Value.Sort(StringComparer.Ordinal);
            }

            return sortedQueryParameters;
        }

        public static SortedDictionary<string, List<string>> SortHeaders(
            HttpRequestHeaders headers,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> defaultHeaders)
        {
            var sortedHeaders = new SortedDictionary<string, List<string>>(StringComparer.Ordinal);

            string FormatHeaderName(string headerName)
            {
                return headerName.ToLowerInvariant();
            }

            void AddHeader(KeyValuePair<string, IEnumerable<string>> header)
            {
                var headerName = FormatHeaderName(header.Key);

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

            void AddDefaultDotnetHeaders()
            {
                foreach (var defaultHeader in defaultHeaders)
                {
                    // On .NET Framework or .NET Core we only add header values if they're not
                    // already added on the message. Note that we don't merge collections: If both
                    // the default headers and the message have set some values for a certain
                    // header, then we don't try to merge the values.
                    if (!sortedHeaders.ContainsKey(FormatHeaderName(defaultHeader.Key)))
                    {
                        AddHeader(defaultHeader);
                    }
                }
            }

            void AddDefaultMonoHeaders()
            {
                foreach (var defaultHeader in defaultHeaders)
                {
                    // On Mono we add header values indifferent of whether the header already exists
                    AddHeader(defaultHeader);
                }
            }

            // Add headers
            foreach (var header in headers)
            {
                AddHeader(header);
            }

            // Add default headers
            if (defaultHeaders != null)
            {
                if (EnvironmentProbe.IsMono)
                {
                    AddDefaultMonoHeaders();
                }
                else
                {
                    AddDefaultDotnetHeaders();
                }
            }

            return sortedHeaders;
        }
    }
}
