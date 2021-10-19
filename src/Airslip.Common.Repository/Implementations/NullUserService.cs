using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Repository.Interfaces;

namespace Airslip.Common.Repository.Implementations
{
    public class NullUserService : IRepositoryUserService
    {
        public string? UserId => null;
    }
}