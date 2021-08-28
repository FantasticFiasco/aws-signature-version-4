// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Authors:
//   Patrik Torstensson (Patrik.Torstensson@labs2.com)
//   Wictor Wilén (decode/encode functions) (wictor@ibizkit.se)
//   Tim Coleman (tim@timcoleman.com)
//   Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// Copyright (C) 2005-2010 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Collections.Specialized;
using System.Text;
using System.Web.Util;

// ReSharper disable once CheckNamespace
namespace System.Web
{
    public sealed class HttpUtility
    {
        private sealed class HttpQSCollection : NameValueCollection
        {
            internal HttpQSCollection()
                : base(StringComparer.OrdinalIgnoreCase)
            {
            }

            public override string ToString()
            {
                int count = Count;
                if (count == 0)
                {
                    return "";
                }

                StringBuilder sb = new StringBuilder();
                string?[] keys = AllKeys;
                for (int i = 0; i < count; i++)
                {
                    string[]? values = GetValues(keys[i]);
                    if (values != null)
                    {
                        foreach (string value in values)
                        {
                            if (string.IsNullOrEmpty(keys[i]))
                            {
                                sb.AppendFormat("{0}&", UrlEncode(value));
                            }
                            else
                            {
                                sb.AppendFormat("{0}={1}&", keys[i], UrlEncode(value));
                            }
                        }
                    }
                }

                return sb.ToString(0, sb.Length - 1);
            }
        }

        public static NameValueCollection ParseQueryString(string query) => ParseQueryString(query, Encoding.UTF8);

        public static NameValueCollection ParseQueryString(string query, Encoding encoding)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            HttpQSCollection result = new HttpQSCollection();
            int queryLength = query.Length;
            int namePos = queryLength > 0 && query[0] == '?' ? 1 : 0;
            if (queryLength == namePos)
            {
                return result;
            }

            while (namePos <= queryLength)
            {
                int valuePos = -1, valueEnd = -1;
                for (int q = namePos; q < queryLength; q++)
                {
                    if (valuePos == -1 && query[q] == '=')
                    {
                        valuePos = q + 1;
                    }
                    else if (query[q] == '&')
                    {
                        valueEnd = q;
                        break;
                    }
                }

                string? name;
                if (valuePos == -1)
                {
                    name = null;
                    valuePos = namePos;
                }
                else
                {
                    name = UrlDecode(query.Substring(namePos, valuePos - namePos - 1), encoding);
                }

                if (valueEnd < 0)
                {
                    valueEnd = query.Length;
                }

                namePos = valueEnd + 1;
                string value = UrlDecode(query.Substring(valuePos, valueEnd - valuePos), encoding);
                result.Add(name, value);
            }

            return result;
        }

        public static string UrlEncode(string str) => UrlEncode(str, Encoding.UTF8);

        public static string UrlEncode(string str, Encoding e) => Encoding.ASCII.GetString(UrlEncodeToBytes(str, e));

        public static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            byte[] bytes = e.GetBytes(str);
            return HttpEncoder.UrlEncode(bytes, 0, bytes.Length, alwaysCreateNewReturnValue: false);
        }

        public static string UrlDecode(string str, Encoding e) => HttpEncoder.UrlDecode(str, e);
    }
}
