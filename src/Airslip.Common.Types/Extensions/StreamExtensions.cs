using System.IO;
using System.Threading.Tasks;

namespace Airslip.Common.Types.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<T> DeserializeStream<T>(this Stream requestBody) where T : class
        {
            StreamReader sr = new(requestBody);
            string payload = await sr.ReadToEndAsync();
            return Json.Deserialize<T>(payload);
        }
        
        public static string ReadStream(this Stream requestBody)
        {
            StreamReader sr = new(requestBody);
            return sr.ReadToEnd();
        }
    }
}