using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Types.Failures;
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
    }
}