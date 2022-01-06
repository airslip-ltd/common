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
        
        Set(result, field, value);
        await SaveChangesAsync();

        return result!;
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
    
    private Expression<Func<T, bool>> _equality<T>(string propertyName, string value)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), propertyName);
        Expression property = Expression.Property(parameter, propertyName);
        Expression target = Expression.Constant(value);
        Expression containsMethod = Expression.Call(property, "Contains", null, target);
        Expression<Func<T, bool>> lambda =
            Expression.Lambda<Func<T, bool>>(containsMethod, parameter);
        return lambda;
    }
    
    private void Set<T, TProperty>(T instance, string propertyName, TProperty value)
    {
        var instanceExpression = Expression.Parameter(typeof(T), "p");
        var propertyGetterExpression = Expression.PropertyOrField(instanceExpression, propertyName);

        //generate setter
        var newValueExpression = Expression.Parameter(typeof(TProperty), "value");
        var assignmentExpression = Expression.Assign(propertyGetterExpression, newValueExpression);
        var lambdaExpression = Expression.Lambda<Action<T, TProperty>>(assignmentExpression, instanceExpression, newValueExpression);
        var setter = lambdaExpression.Compile();// the generated lambda will look like so: (p, value) => p.{your_property_name} = value;
        setter(instance, value);
    }
}