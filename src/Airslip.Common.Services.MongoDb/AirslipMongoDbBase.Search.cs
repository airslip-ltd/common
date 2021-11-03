using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
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
            IMongoCollection<TEntity> collection = CollectionByType<TEntity>();

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