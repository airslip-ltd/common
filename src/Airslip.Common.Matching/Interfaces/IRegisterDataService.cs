using System.Threading.Tasks;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IRegisterDataService<in TType> where TType : class
    {
        Task RegisterData(TType model);
        Task RegisterData(string message);
    }
}