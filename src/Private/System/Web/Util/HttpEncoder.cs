// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET45
#pragma warning disable IDE0007 // Use implicit type

using System.Diagnostics;
using System.Text;

// ReSharper disable once CheckNamespace
namespace System.Web.Util
{
    internal static class HttpEncoder
    {
        internal static string UrlDecode(string value, Encoding encoding)
        {
            int count = value.Length;
            UrlDecoder helper = new(count, encoding);

            // go through the string's chars collapsing %XX and %uXXXX and
            // appending each char as char, with exception of %XX constructs
            // that are appended as bytes

            for (int pos = 0; pos < count; pos++)
            {
                char ch = value[pos];

                if (ch == '+')
                {
                    ch = ' ';
                }
                else if (ch == '%' && pos < count - 2)
                {
                    if (value[pos + 1] == 'u' && pos < count - 5)
                    {
                        int h1 = HttpEncoderUtility.HexToInt(value[pos + 2]);
                        int h2 = HttpEncoderUtility.HexToInt(value[pos + 3]);
                        int h3 = HttpEncoderUtility.HexToInt(value[pos + 4]);
                        int h4 = HttpEncoderUtility.HexToInt(value[pos + 5]);

                        if (h1 >= 0 && h2 >= 0 && h3 >= 0 && h4 >= 0)
                        {   // valid 4 hex chars
                            ch = (char)((h1 << 12) | (h2 << 8) | (h3 << 4) | h4);
                            pos += 5;

                            // only add as char
                            helper.AddChar(ch);
                            continue;
                        }
                    }
                    else
                    {
                        int h1 = HttpEncoderUtility.HexToInt(value[pos + 1]);
                        int h2 = HttpEncoderUtility.HexToInt(value[pos + 2]);

                        if (h1 >= 0 && h2 >= 0)
                        {     // valid 2 hex chars
                            byte b = (byte)((h1 << 4) | h2);
                            pos += 2;

                            // don't add as char
                            helper.AddByte(b);
                            continue;
                        }
                    }
                }

                if ((ch & 0xFF80) == 0)
                {
                    helper.AddByte((byte)ch); // 7 bit have to go as bytes because of Unicode
                }
                else
                {
                    helper.AddChar(ch);
                }
            }

            return Utf16StringValidator.ValidateString(helper.GetString());
        }

        internal static byte[]? UrlEncode(byte[] bytes, int offset, int count, bool alwaysCreateNewReturnValue)
        {
            byte[]? encoded = UrlEncode(bytes, offset, count);

            return (alwaysCreateNewReturnValue && (encoded != null) && (encoded == bytes))
                ? (byte[])encoded.Clone()
                : encoded;
        }

        private static byte[]? UrlEncode(byte[] bytes, int offset, int count)
        {
            if (!ValidateUrlEncodingParameters(bytes, offset, count))
            {
                return null;
            }

            int cSpaces = 0;
            int cUnsafe = 0;

            // count them first
            for (int i = 0; i < count; i++)
            {
                char ch = (char)bytes[offset + i];

                if (ch == ' ')
                {
                    cSpaces++;
                }
                else if (!HttpEncoderUtility.IsUrlSafeChar(ch))
                {
                    cUnsafe++;
                }
            }

            // nothing to expand?
            if (cSpaces == 0 && cUnsafe == 0)
            {
                // DevDiv 912606: respect "offset" and "count"
                if (0 == offset && bytes.Length == count)
                {
                    return bytes;
                }
                else
                {
                    byte[] subarray = new byte[count];
                    Buffer.BlockCopy(bytes, offset, subarray, 0, count);
                    return subarray;
                }
            }

            // expand not 'safe' characters into %XX, spaces to +s
            byte[] expandedBytes = new byte[count + cUnsafe * 2];
            int pos = 0;

            for (int i = 0; i < count; i++)
            {
                byte b = bytes[offset + i];
                char ch = (char)b;

                if (HttpEncoderUtility.IsUrlSafeChar(ch))
                {
                    expandedBytes[pos++] = b;
                }
                else if (ch == ' ')
                {
                    expandedBytes[pos++] = (byte)'+';
                }
                else
                {
                    expandedBytes[pos++] = (byte)'%';
                    expandedBytes[pos++] = (byte)HttpEncoderUtility.IntToHex((b >> 4) & 0xf);
                    expandedBytes[pos++] = (byte)HttpEncoderUtility.IntToHex(b & 0x0f);
                }
            }

            return expandedBytes;
        }

        private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            if (offset < 0 || offset > bytes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            if (count < 0 || offset + count > bytes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return true;
        }

        // Internal class to facilitate URL decoding -- keeps char buffer and byte buffer, allows appending of either chars or bytes
        private class UrlDecoder
        {
            private readonly int _bufferSize;

            // Accumulate characters in a special array
            private int _numChars;
            private readonly char[] _charBuffer;

            // Accumulate bytes for decoding into characters in a special array
            private int _numBytes;
            private byte[]? _byteBuffer;

            // Encoding to convert chars to bytes
            private readonly Encoding _encoding;

            private void FlushBytes()
            {
                if (_numBytes > 0)
                {
                    Debug.Assert(_byteBuffer != null);
                    _numChars += _encoding.GetChars(_byteBuffer, 0, _numBytes, _charBuffer, _numChars);
                    _numBytes = 0;
                }
            }

            internal UrlDecoder(int bufferSize, Encoding encoding)
            {
                _bufferSize = bufferSize;
                _encoding = encoding;

                _charBuffer = new char[bufferSize];
                // byte buffer created on demand
            }

            internal void AddChar(char ch)
            {
                if (_numBytes > 0)
                {
                    FlushBytes();
                }

                _charBuffer[_numChars++] = ch;
            }

            internal void AddByte(byte b)
            {
                // if there are no pending bytes treat 7 bit bytes as characters
                // this optimization is temp disable as it doesn't work for some encodings
                /*
                                if (_numBytes == 0 && ((b & 0x80) == 0)) {
                                    AddChar((char)b);
                                }
                                else
                */
                {
                    if (_byteBuffer == null)
                    {
                        _byteBuffer = new byte[_bufferSize];
                    }

                    _byteBuffer[_numBytes++] = b;
                }
            }

            internal string GetString()
            {
                if (_numBytes > 0)
                {
                    FlushBytes();
                }

                return _numChars > 0 ? new string(_charBuffer, 0, _numChars) : "";
            }
        }
    }
}

#pragma warning restore IDE0007 // Use implicit type
#endif
