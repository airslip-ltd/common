using Airslip.Common.Repository.Types.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Types.Interfaces
{
    /// <summary>
    /// An interface defining the how daya will be searched and returned.
    ///  By defining it here we remove any specific requirements pertaining to a
    ///  particular context type, meaning a context could be anything
    /// </summary>
    public interface ISearchContext
    {
        /// <summary>
        /// Returns a list of entities based on search criteria
        /// </summary>
        /// <param name="searchFilters">Search filters to apply to the search</param>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <returns>A list of entities matching the search criteria</returns>
        Task<List<TEntity>> SearchEntities<TEntity>(List<SearchFilterModel> searchFilters)
            where TEntity : class, IEntityWithId;
    }
}