using System.IO;
using System.Threading.Tasks;

namespace Airslip.Common.Utilities.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<T> DeserializeStream<T>(this Stream requestBody) where T : class
        {
            StreamReader sr = new(requestBody);
            string payload = await sr.ReadToEndAsync();
            return Json.Deserialize<T>(payload);
        }

        public static async Task<T> DeserializeStream<T>(this Stream stream, bool resetStream) where T : class
        {
            StreamReader sr = new(stream);
            string payload = await sr.ReadToEndAsync();
            if (resetStream)
                stream.Position = 0;
            return Json.Deserialize<T>(payload);
        }

        public static string ReadStream(this Stream requestBody)
        {
            StreamReader sr = new(requestBody);
            return sr.ReadToEnd();
        }

        public static string ReadStream(this Stream stream, bool resetStream)
        {
            StreamReader sr = new(stream);
            
            string payload = sr.ReadToEnd();
            
            if (resetStream)
                stream.Position = 0;

            return payload;
        }
    }
}