using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatServer
{
    internal static class BinaryReaderExtensions
    {
        public static string ReadAllUTF8Text(this BinaryReader @this)
        {
            return Encoding.UTF8.GetString(
                @this.ReadBytes((int)@this.BaseStream.Length)
                    .Where(b => b != '\0')
                    .ToArray()
            );
        }
    }

    public static class StringExtensions
    {
        public static bool Contains(this string @this, string substring, StringComparison comparison)
        {
            return @this?.IndexOf(substring, comparison) >= 0; 
        }

        public static bool IsEmpty(this string @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }

        /// <summary>
        /// Extracts a recipient name from the string value. The string value must be
        /// formatted as follows: "@RECIPIENT:MESSAGE".
        /// </summary>
        /// <param name="this">Input string.</param>
        /// <returns>RECIPIENT value from the string formatted as follows: "@RECIPIENT:MESSAGE".</returns>
        public static string TryExtractRecipient(this string @this)
        {
            string recipient = null;

            if(!@this.IsEmpty() && @this.StartsWith("@"))
            {
                var recipientRegEx = new Regex(@"@(\w+):");
                var match = recipientRegEx.Match(@this.TrimStart());

                if(match.Success)
                {
                    recipient = match.Groups[1].Value;
                }
            }

            return recipient;
        }
    }
}
