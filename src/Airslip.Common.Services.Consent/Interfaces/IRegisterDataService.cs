using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.Consent.Enums;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IRegisterDataService<TEntity, in TModel> 
        where TModel : class, IModel, IFromDataSource
        where TEntity : class, IEntity, IFromDataSource
    {
        Task RegisterData(TModel model, DataSources dataSource);
        Task RegisterData(string message, DataSources dataSource);
    }
}