using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.Models
{
    public record NotFoundResultModel<TModel>(string ErrorCode)
        : RepositoryActionResultModel<TModel>, IFail
        where TModel : class, IModel;
}