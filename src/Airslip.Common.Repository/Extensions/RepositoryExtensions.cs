using Airslip.Common.Repository.Enums;

namespace Airslip.Common.Repository.Extensions
{
    public static class RepositoryExtensions
    {
        public static bool IsActive(this EntityStatus entityStatus) 
            => entityStatus == EntityStatus.Active;
    }
}