using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.Models
{
    public record FailedVerificationResultModel<TModel>(string ErrorCode, 
            TModel? PreviousVersion = null,
            ValidationResultModel? ValidationResult = null)
        : RepositoryActionResultModel<TModel>(null, PreviousVersion, ValidationResult), IFail
        where TModel : class, IModel;
}