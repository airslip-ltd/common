using Airslip.Common.Repository.Types.Interfaces;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Interfaces
{
    /// <summary>
    /// Interface to define a custom entity search formatter. This would be used if you want to extend
    ///  the data returned from a search query to include other properties or to format data before
    ///  sending it to the client
    /// </summary>
    /// <typeparam name="TModel">The model type this formatter is used for</typeparam>
    public interface IEntitySearchFormatter<TModel>
        where TModel : class, IModel
    {
        /// <summary>
        /// The function called to format the model
        /// </summary>
        /// <param name="model">The model that has been generated by the search process</param>
        /// <returns>An updated version of the model</returns>
        Task<TModel> FormatModel(TModel model);
    }
}