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
        /// <param name="entitySearch">The search query model</param>
        /// <param name="mandatoryFilters">Mandatory filters for defining data ownership</param>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <returns>A list of entities matching the search criteria</returns>
        Task<EntitySearchResult<TEntity>> SearchEntities<TEntity>(EntitySearchQueryModel entitySearch, 
            List<SearchFilterModel> mandatoryFilters)
            where TEntity : class, IEntityWithId;
    
        Task<int> RecordCount<TEntity>(EntitySearchQueryModel entitySearch, 
            List<SearchFilterModel> mandatoryFilters)
            where TEntity : class, IEntityWithId;
    }
}