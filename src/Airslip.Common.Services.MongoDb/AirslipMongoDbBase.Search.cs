using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Services.MongoDb.Extensions;
using Airslip.Common.Types.Enums;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{
    public abstract partial class AirslipMongoDbBase
    {
        public Task<List<TEntity>> GetEntities<TEntity>(List<SearchFilterModel> searchFilters)
            where TEntity : class, IEntityWithId
        {

            if (typeof(IEntityWithOwnership).IsAssignableFrom(typeof(TEntity)))
            {
                switch (_userContext.AirslipUserType ?? AirslipUserType.Standard)
                {
                    case AirslipUserType.Standard:
                        searchFilters.Add(new SearchFilterModel("userId", _userContext.UserId!));
                        break;
                    default:
                        searchFilters.Add(new SearchFilterModel("entityId", 
                            _userContext.EntityId!));
                        searchFilters.Add(new SearchFilterModel("airslipUserType", 
                            _userContext.AirslipUserType!));
                        break;
                } 
            }
            
            
            
            
            
            
            IMongoCollection<TEntity> collection = Database.CollectionByType<TEntity>();

            FilterDefinitionBuilder<TEntity>? filterBuilder = Builders<TEntity>.Filter;
            List<FilterDefinition<TEntity>> filters = searchFilters
                .Select(searchFilterModel => filterBuilder
                    .Eq(searchFilterModel.FieldName, searchFilterModel.FieldValue))
                .ToList();

            return collection
                .Find(filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<TEntity>.Empty)
                .ToListAsync();
        }
    }
}