using System.IO;
using System.Threading.Tasks;

namespace Airslip.Common.Contracts
{
    public interface IStorage<in T> where T : class
    {
        Task SaveFileAsync(T value);
        Task<(Stream, string?)> DownloadToStreamAsync(string name);
    }
}