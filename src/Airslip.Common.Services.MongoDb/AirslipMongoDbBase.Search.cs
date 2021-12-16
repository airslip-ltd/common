using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Services.MongoDb.Extensions;
using Airslip.Common.Types.Enums;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{
    public abstract partial class AirslipMongoDbBase
    {
        public Task<List<TEntity>> SearchEntities<TEntity>(List<SearchFilterModel> searchFilters)
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

            FilterDefinitionBuilder<TEntity>? filterBuilder = Builders<TEntity>.Filter;
            List<FilterDefinition<TEntity>> filters = new();
            foreach (SearchFilterModel searchFilterModel in searchFilters)
            {
                switch (searchFilterModel.FieldValue)
                {
                    case bool boolValue:
                        filters.Add(filterBuilder.Eq(searchFilterModel.FieldName, boolValue));
                        break;
                    case int intValue:
                        filters.Add(filterBuilder.Eq(searchFilterModel.FieldName, intValue));
                        break;
                    case long lngValue:
                        filters.Add(filterBuilder.Eq(searchFilterModel.FieldName, lngValue));
                        break;
                    case AirslipUserType airslipUserType:
                        filters.Add(filterBuilder.Eq(searchFilterModel.FieldName, airslipUserType));
                        break;
                    default:
                        filters.Add(filterBuilder.Eq(searchFilterModel.FieldName, searchFilterModel.FieldValue
                            .ToString()));
                        break;
                }
            }

            IMongoCollection<TEntity> collection = Database.CollectionByType<TEntity>();

            return collection
                .Find(filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<TEntity>.Empty)
                .ToListAsync();
        }
    }
}