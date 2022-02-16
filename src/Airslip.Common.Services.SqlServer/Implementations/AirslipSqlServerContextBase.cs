using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Airslip.Common.Services.SqlServer.Extensions;

namespace Airslip.Common.Services.SqlServer.Implementations;

public abstract class AirslipSqlServerContextBase : DbContext, ISearchContext, IContext
{
    protected AirslipSqlServerContextBase(DbContextOptions options)
        : base(options)
    {
        
    }

    public async Task<TEntity> AddEntity<TEntity>(TEntity newEntity) where TEntity : class, IEntityWithId
    {
        EntityEntry<TEntity> result = await Set<TEntity>().AddAsync(newEntity);
        await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<TEntity?> GetEntity<TEntity>(string id) where TEntity : class, IEntityWithId
    {
        TEntity? result = await Set<TEntity>().FindAsync(id);
        return result;
    }

    public async Task<TEntity> UpdateEntity<TEntity>(TEntity updatedEntity) where TEntity : class, IEntityWithId
    {
        EntityEntry<TEntity> updateResult = Update(updatedEntity);
        await SaveChangesAsync();
        return updateResult.Entity;
    }

    public IQueryable<TEntity> QueryableOf<TEntity>() where TEntity : class
    {
        return Set<TEntity>().AsQueryable();
    }

    public async Task<TEntity> UpsertEntity<TEntity>(TEntity newEntity) where TEntity : class, IEntityWithId
    {
        IQueryable<string> q = from table in Set<TEntity>().AsQueryable()
            where table.Id == newEntity.Id
            select table.Id;

        TEntity result;
        
        if (!await q.AnyAsync())
        {
            result =  await AddEntity(newEntity);
        }
        else
        {
            result =  await UpdateEntity(newEntity);
        }
        
        return result;
    }

    public async Task<TEntity> Update<TEntity>(string id, string field, string value) where TEntity : class, IEntityWithId
    {
        TEntity? result = await Set<TEntity>().FindAsync(id);

        if (result == null) return result!;
        
        _set(result, field, value);
        await SaveChangesAsync();

        return result;
    }
    
    private static void _set<T, TProperty>(T instance, string propertyName, TProperty value)
    {
        ParameterExpression instanceExpression = Expression.Parameter(typeof(T), "p");
        MemberExpression propertyGetterExpression = Expression.PropertyOrField(instanceExpression, propertyName);
        ParameterExpression newValueExpression = Expression.Parameter(typeof(TProperty), "value");
        BinaryExpression assignmentExpression = Expression.Assign(propertyGetterExpression, newValueExpression);
        Expression<Action<T, TProperty>> lambdaExpression = Expression.Lambda<Action<T, TProperty>>(assignmentExpression, instanceExpression, newValueExpression);
        Action<T, TProperty> setter = lambdaExpression.Compile();
        setter(instance, value);
    }

    public async Task<EntitySearchResult<TEntity>> SearchEntities<TEntity>(EntitySearchQueryModel entitySearch, List<SearchFilterModel> mandatoryFilters) 
        where TEntity : class, IEntityWithId
    {
        IQueryable<TEntity> query = Set<TEntity>().BuildQuery(entitySearch, mandatoryFilters);

        int count = await query.CountAsync();

        if (entitySearch.Page > 0)
            query = query.Skip(entitySearch.Page * entitySearch.RecordsPerPage);
        
        if (entitySearch.RecordsPerPage > 0)
            query = query.Take(entitySearch.RecordsPerPage);

        foreach (EntitySearchSortModel sortModel in entitySearch.Sort)
        {
            query = sortModel.Sort == SortOrder.Asc ? query.OrderBy(sortModel.Field) : query.OrderByDescending(sortModel.Field);
        }
        
        List<TEntity> list = await query
            .ToListAsync();

        return new EntitySearchResult<TEntity>(list, count);
    }

    public async Task<int> RecordCount<TEntity>(EntitySearchQueryModel entitySearch, List<SearchFilterModel> mandatoryFilters) where TEntity : class, IEntityWithId
    {
        return await Set<TEntity>().BuildQuery(entitySearch,  mandatoryFilters)
            .CountAsync();
    }
}