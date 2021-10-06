using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.Models
{
    public record SuccessfulActionResultModel<TModel>(TModel? CurrentVersion = null,
            TModel? PreviousVersion = null,
            ValidationResultModel? ValidationResult = null)
        : RepositoryActionResultModel<TModel>(ResultType.Success, CurrentVersion, PreviousVersion, ValidationResult), ISuccess
        where TModel : class, IModel;
}