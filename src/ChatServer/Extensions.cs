using System;
using System.IO;
using System.Linq;
using System.Text;

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

    internal static class StringExtensions
    {
        public static bool Contains(this string @this, string substring, StringComparison comparison)
        {
            return @this?.IndexOf(substring, comparison) >= 0; 
        }
    }
}
