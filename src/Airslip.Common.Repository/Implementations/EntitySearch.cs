using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations;

/// <summary>
/// Generic implementation of entity search, designed to allow for quickly creating new APIs with search capability
/// </summary>
/// <typeparam name="TEntity">The entity type we will be searching for</typeparam>
/// <typeparam name="TModel">The model type we will be returning</typeparam>
public class EntitySearch<TModel> : IEntitySearch<TModel> 
    where TModel : class, IModel
{
    private readonly ISearchContext _context;
    private readonly IModelMapper<TModel> _mapper;
    private readonly IEnumerable<IEntitySearchFormatter<TModel>> _searchFormatters;
        
    public EntitySearch(ISearchContext context, IModelMapper<TModel> mapper, 
        IEnumerable<IEntitySearchFormatter<TModel>> searchFormatters)
    {
        _context = context;
        _mapper = mapper;
        _searchFormatters = searchFormatters;
    }
        
    /// <summary>
    /// Singe function that takes a list of search filters and returns a list of formatted models
    /// </summary>
    /// <param name="entitySearch">The query model sent from the user</param>
    /// <param name="mandatoryFilters">Mandatory filters used for applying server side
    /// filtering such as context sensitive user / entity</param>
    /// <returns>A list of formatted models</returns>
    public async Task<EntitySearchResponse<TModel>> GetSearchResults<TEntity>(EntitySearchQueryModel entitySearch, 
        List<SearchFilterModel> mandatoryFilters) where TEntity : class, IEntity
    {
        // Get search results for our entities
        EntitySearchResult<TEntity> searchResults = await _context
            .SearchEntities<TEntity>(entitySearch, mandatoryFilters);
        
        EntitySearchResponse<TModel> pagedResult = new()
        {
            Paging = entitySearch.CalculatePagination(searchResults.RecordCount)
        };

        // Format them into models
        foreach (TEntity result in searchResults.Records)
        {
            // Create a new model using the mapper
            TModel newModel = _mapper.Create(result);

            // If we have a search formatter we can use it here to populate any additional data
            foreach (IEntitySearchFormatter<TModel> entitySearchFormatter in _searchFormatters)
            {
                newModel = await entitySearchFormatter.FormatModel(newModel);
            }

            // Add to the list
            pagedResult.Results.Add(newModel);
        }
            
        return pagedResult;
    }
}