//-----------------------------------------------------------------------
// <copyright file="Cipher.cs" company="https://dudley.codes">
// This is free and unencumbered software released into the public domain. For more information, 
// please refer to http://unlicense.org/
//
// Anyone is free to copy, modify, publish, use, compile, sell, or distribute this software, either
// in source code form or as a compiled binary, for any purpose, commercial or non-commercial, and
// by any means.
//
// In jurisdictions that recognize copyright laws, the author or authors of this software dedicate
// any and all copyright interest in the software to the public domain.We make this dedication for 
// the benefit of the public at large and to the detriment of our heirs and successors.We intend 
// this dedication to be an overt act of relinquishment in perpetuity of all present and future 
// rights to this software under copyright law.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <author>James Dudley</author>
//-----------------------------------------------------------------------

namespace DudleyCodes
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>Class to encode to and from Base64</summary>
    public static class Cipher
    {
        /// <summary>Used by <see cref="Cipher.Base64UrlEncode()"/> and <see cref="Cipher.Base64UrlDecode()"/> to generate padding.
        /// Should be static but may prefer to be unique among different applications.</summary>
        internal static string Base64UrlNoise = "DEFGa";

        /// <summary>Decode a Base64 string to its original value.</summary>
        /// <param name="base64EncodedData">String to decode.</param>
        /// <returns>String containing the original value.</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>Encode a string to Base64. Generates text that only contains upper and lower case letters (A-Z, a-z),
        /// numerals (0-9), and the "+", "/", and "=" symbols.</summary>
        /// <param name="plainText">String to encode.</param>
        /// <returns>ASCII string containing encoded value.</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>Reverse (decode) of <see cref="Cipher.Base64UrlEncode"/>.</summary>
        /// <param name="value">Value to decode.</param>
        /// <returns>Decoded string value.</returns>
        /// <remarks>See also <see cref="Base64UrlEncode"/></remarks>
        public static string Base64UrlDecode(string value)
        {
            value = value.Substring(1, value.Length - 2);
            value = value.Replace('-', '+').Replace('_', '/').Replace('.', '=');
            return Cipher.Base64Decode(value);
        }

        /// <summary>Encodes (obfuscates) a value for use in a URL query or fragment.</summary>
        /// <param name="plainText">Value to encode.</param>
        /// <returns>Encoded string value.</returns>
        /// /// <remarks>See also <see cref="Base64UrlDecode"/></remarks>
        public static string Base64UrlEncode(string plainText)
        {
            string encodedText = Cipher.Base64Encode(plainText);

            //// Replace Base64 symbols that are not URL-fragment friendly with characters that are
            encodedText = encodedText.Replace('+', '-').Replace('/', '_').Replace('=', '.');

            //// Pad seemingly random but static characters to ensure non-alphanumeric symbols don't
            MD5 md5 = MD5.Create();
            byte[] staticNoise = md5.ComputeHash(Encoding.ASCII.GetBytes(plainText + "aS4LT" + Cipher.Base64UrlNoise));
            string padding = Encoding.ASCII.GetString(staticNoise).ToUpper();

            return padding[2] + encodedText + padding[11];
        }
    }
}
