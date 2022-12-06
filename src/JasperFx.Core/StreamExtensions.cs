namespace JasperFx.Core
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Read the contents of a Stream from its current location
        /// into a String
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadAllText(this Stream stream)
        {
            using var sr = new StreamReader(stream, leaveOpen: true);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Read all the bytes in a Stream from its current
        /// location to a byte[] array
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            using var content = new MemoryStream();
            stream.CopyTo(content);
            return content.ToArray();
        }

        /// <summary>
        /// Asynchronously read the contents of a Stream from its current location
        /// into a String
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Task<string> ReadAllTextAsync(this Stream stream)
        {
            using var sr = new StreamReader(stream, leaveOpen: true);
            return sr.ReadToEndAsync();
        }

        /// <summary>
        /// Asynchronously read all the bytes in a Stream from its current
        /// location to a byte[] array
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<byte[]> ReadAllBytesAsync(this Stream stream)
        {
            using var content = new MemoryStream();
            await stream.CopyToAsync(content).ConfigureAwait(false);
            return content.ToArray();
        }
    }
}