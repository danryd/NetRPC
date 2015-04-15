namespace NetRPC
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    /// <summary>
    /// A class that encapsulates how we interact with the I/O stream.
    /// </summary>
    public class StreamHandler
    {
        /// <summary>
        /// Writes all bytes to a stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="stream"></param>
        public void WriteToStream(byte[] bytes, Stream stream)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Reads from stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public byte[] ReadAllBytes(Stream stream)
        {
            var rqBytes = new byte[4096];
            List<byte> all = new List<byte>();
            var rep = 0;
            while (stream.Read(rqBytes, 4096 * rep, 4096) > 0)
            {
                all.AddRange(rqBytes);
            }
            return all.ToArray();
        }

        public string ReadToString(Stream stream)
        {
            return Encoding.UTF8.GetString(ReadAllBytes(stream));
        }
    }
}
