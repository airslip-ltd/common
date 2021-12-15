using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Repository.Extensions
{
    public static class RepositoryExtensions
    {
        public static bool IsActive(this EntityStatus entityStatus) 
            => entityStatus == EntityStatus.Active;
        
        public static ErrorResponses ToUnsuccessfulResponse<TModel>(
            this RepositoryActionResultModel<TModel> result, string errorCode = "400") where TModel : class, IModel
        {
            ICollection<ErrorResponse> errors = result.ValidationResult!.Results
                .Select(v => new ErrorResponse(errorCode, v.Message))
                .ToList();

            return new ErrorResponses(errors);
        }

        public static bool CanView<TEntity>(this TEntity entity, IUserContext userService)
            where TEntity: class, IEntityWithOwnership
        {
            if (userService.AirslipUserType is null) return false;
            if (userService.UserId is null) return false;
            if (entity.AirslipUserType != userService.AirslipUserType) return false;
            if (entity.AirslipUserType == AirslipUserType.Standard && entity.UserId != userService.UserId) return false;
            if (entity.AirslipUserType != AirslipUserType.Standard && entity.EntityId != userService.EntityId) return false;

            return true;
        }
    }
}