// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace System.Web.Util
{
    internal static class UriUtil
    {
        private static readonly char[] s_queryFragmentSeparators = { '?', '#' };

        // Just extracts the query string and fragment from the input path by splitting on the separator characters.
        // Doesn't perform any validation as to whether the input represents a valid URL.
        // Concatenating the pieces back together will form the original input string.
        private static void ExtractQueryAndFragment(string input, out string path, out string? queryAndFragment)
        {
            int queryFragmentSeparatorPos = input.IndexOfAny(s_queryFragmentSeparators);
            if (queryFragmentSeparatorPos != -1)
            {
                path = input.Substring(0, queryFragmentSeparatorPos);
                queryAndFragment = input.Substring(queryFragmentSeparatorPos);
            }
            else
            {
                // no query or fragment separator
                path = input;
                queryAndFragment = null;
            }
        }
    }
}
