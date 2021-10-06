using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.Models
{
    public abstract record RepositoryActionResultModel<TModel>
    (TModel? CurrentVersion = null,
        TModel? PreviousVersion = null,
        ValidationResultModel? ValidationResult = null) : IResponse
        where TModel : class, IModel;
}