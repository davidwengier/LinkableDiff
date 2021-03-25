using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LinkableDiff
{
    internal static class Compressor
    {
        public static string Compress(string version1, string version2)
        {
            var separator = (char)7;
            return Compress(version1 + separator + version2);

            static string Compress(string input)
            {
                using var ms = new MemoryStream();
                using (var compressor = new DeflateStream(ms, CompressionLevel.Optimal))
                {
                    var inputBytes = Encoding.Unicode.GetBytes(input);
                    compressor.Write(inputBytes);
                }
                return Convert.ToBase64String(ms.ToArray()).Replace("/", "$");
            }
        }

        public static string Uncompress(string slug)
        {
            try
            {
                var bytes = Convert.FromBase64String(slug.Replace("$", "/"));

                using var ms = new MemoryStream(bytes);
                using (var compressor = new DeflateStream(ms, CompressionMode.Decompress))
                using (var sr = new StreamReader(compressor, Encoding.Unicode))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
