using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Interfaces;

public interface IModelPreProcessEvent<TModel> : IModelProcessEvent<TModel>
    where TModel : class, IModel
{
    
}