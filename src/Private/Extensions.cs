using System;
using System.Collections.Generic;
using System.Net.Http;
using Amazon.Runtime.Internal.Auth;
using Amazon.Util;

namespace AwsSignatureVersion4.Private
{
    public static class Extensions
    {
        private const char SpaceCharacter = ' ';

        /// <summary>
        /// Converts string into an instance of an <see cref="Uri"/>.
        /// </summary>
        public static Uri ToUri(this string self) => new(self, UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Normalize string by reducing multiple sequential whitespaces into a single space.
        /// </summary>
        public static string NormalizeWhiteSpace(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return string.Empty;
            }

            var currentIndex = 0;
            var skipped = false;
            var output = new char[self.Length];

            foreach (var currentChar in self.ToCharArray())
            {
                if (char.IsWhiteSpace(currentChar))
                {
                    if (!skipped)
                    {
                        if (currentIndex > 0)
                        {
                            output[currentIndex++] = SpaceCharacter;
                        }

                        skipped = true;
                    }
                }
                else
                {
                    skipped = false;
                    output[currentIndex++] = currentChar;
                }
            }

            return new string(output, 0, currentIndex);
        }

        /// <summary>
        /// Adds header with single value.
        /// </summary>
        public static void AddHeader(this HttpRequestMessage request, string headerName, string headerValue)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            request.Headers.Add(headerName, headerValue);
        }

        /// <summary>
        /// Adds header with multiple values.
        /// </summary>
        public static void AddHeaders(this HttpRequestMessage request, string headerName, IEnumerable<string> headerValues)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            request.Headers.Add(headerName, headerValues);
        }

        /// <summary>
        /// Adds header if <paramref name="condition"/> is true.
        /// </summary>
        public static void AddHeaderIf(this HttpRequestMessage request, bool condition, string headerName, string headerValue)
        {
            if (condition)
            {
                AddHeader(request, headerName, headerValue);
            }
        }

        /// <summary>
        /// Converts instance into a ISO8601 basic date format of 'yyyyMMdd'.
        /// </summary>
        public static string ToIso8601BasicDate(this DateTime self) =>
            AWS4Signer.FormatDateTime(self, AWSSDKUtils.ISO8601BasicDateFormat);

        /// <summary>
        /// Converts instance into a ISO8601 basic date/time format of 'yyyyMMddTHHmmssZ'.
        /// </summary>
        public static string ToIso8601BasicDateTime(this DateTime self) =>
            AWS4Signer.FormatDateTime(self, AWSSDKUtils.ISO8601BasicDateTimeFormat);
    }
}
