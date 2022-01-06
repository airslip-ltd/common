using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
    
    public async Task<List<TEntity>> SearchEntities<TEntity>(List<SearchFilterModel> searchFilters) where TEntity : class, IEntityWithId
    {
        IQueryable<TEntity> q = Set<TEntity>().AsQueryable();

        foreach (SearchFilterModel searchFilterModel in searchFilters)
        {
            Expression<Func<TEntity, bool>> lambda = 
                _equality<TEntity>(searchFilterModel.FieldName, searchFilterModel.FieldValue);
            q = q.Where(lambda);   
        }

        return await q.ToListAsync();
    }
    
    private static Expression<Func<T, bool>> _equality<T>(string propertyName, string value)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), propertyName);
        Expression property = Expression.Property(parameter, propertyName);
        Expression target = Expression.Constant(value);
        Expression containsMethod = Expression.Call(property, "Contains", null, target);
        Expression<Func<T, bool>> lambda =
            Expression.Lambda<Func<T, bool>>(containsMethod, parameter);
        return lambda;
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
}