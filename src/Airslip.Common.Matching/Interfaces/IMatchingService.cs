using System.Threading.Tasks;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IMatchingService
    {
        Task TryMatch(string myQueueItem);
    }
}