using Airslip.Common.Repository.Types.Constants;
using Airslip.Common.Repository.Types.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Airslip.Common.Services.SqlServer.Extensions;

internal static class IQueryableExtensions 
{
    internal static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderBy(ToLambda<T>(propertyName));
    }

    internal static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderByDescending(ToLambda<T>(propertyName));
    }

    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T));
        MemberExpression property = Expression.Property(parameter, propertyName);
        UnaryExpression propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);            
    }

    private static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        // need to detect whether they use the same
        // parameter instance; if not, they need fixing
        ParameterExpression param = expr1.Parameters[0];
        if (ReferenceEquals(param, expr2.Parameters[0]))
        {
            // simple version
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(expr1.Body, expr2.Body), param);
        }
        // otherwise, keep expr1 "as is" and invoke expr2
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                expr1.Body,
                Expression.Invoke(expr2, param)), param);
    }

    private static Expression<Func<T, bool>> OrElse<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        // need to detect whether they use the same
        // parameter instance; if not, they need fixing
        ParameterExpression param = expr1.Parameters[0];
        if (ReferenceEquals(param, expr2.Parameters[0]))
        {
            // simple version
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(expr1.Body, expr2.Body), param);
        }
        // otherwise, keep expr1 "as is" and invoke expr2
        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(
                expr1.Body,
                Expression.Invoke(expr2, param)), param);
    }

    private static Expression<Func<TEntity, bool>> CreateLambdaExpression<TEntity>(this SearchFilterModel filterModel)
    {
        ParameterExpression arg = Expression.Parameter(typeof(TEntity), "p");
        PropertyInfo? property = typeof(TEntity).GetProperty(filterModel.ColumnField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        dynamic compareValue = Expression.Constant(GetTypedValue(filterModel.Value, property!.PropertyType));
        MemberExpression member = Expression.MakeMemberAccess(arg, property);
        
        Expression comparison;
        switch (filterModel.OperatorValue)
        {
            case Operators.OPERATOR_CONTAINS:
                comparison = Expression.Call(
                    member,
                    "Contains",
                    null, 
                    compareValue);
                break;
            case Operators.OPERATOR_EQUALS:
                comparison = Expression.Equal(
                    member,
                    compareValue);
                break;
            case Operators.OPERATOR_GREATER_THAN_EQUALS:
                comparison = Expression.GreaterThanOrEqual(
                    member,
                    compareValue);
                break;
            case Operators.OPERATOR_LESS_THAN_EQUALS:
                comparison = Expression.LessThanOrEqual(member, compareValue);
                break;
            case Operators.OPERATOR_GREATER_THAN:
                comparison = Expression.LessThan(member, compareValue);
                break;
            case Operators.OPERATOR_LESS_THAN:
                comparison = Expression.GreaterThan(member, compareValue);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return Expression.Lambda<Func<TEntity, bool>>(comparison, arg);
    }

    private static dynamic GetTypedValue(dynamic value, Type targetType)
    {
        return targetType.IsEnum ? 
            Enum.Parse(targetType, (string) value) : 
            Convert.ChangeType(value, targetType);
    }
    
    internal static IQueryable<TEntity> BuildQuery<TEntity>(this DbSet<TEntity> set, EntitySearchQueryModel entitySearch, 
        List<SearchFilterModel> mandatoryFilters) where TEntity : class
    {
        IQueryable<TEntity> q = set.AsQueryable();
        
        Expression<Func<TEntity, bool>>? lambda = mandatoryFilters.BuildFilterQuery<TEntity>();
        if (lambda != null) q = q.Where(lambda);
        
        lambda = entitySearch.Search?.Items.BuildFilterQuery<TEntity>(entitySearch.Search.LinkOperator);
        if (lambda != null) q = q.Where(lambda);
        
        return q;
    }

    private static Expression<Func<TEntity, bool>>? BuildFilterQuery<TEntity>(this List<SearchFilterModel> filters, 
        string linkOperator = Operators.LINK_OPERATOR_AND)
    {
        Expression<Func<TEntity, bool>>? lambda = null;
        Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>>? 
            predicate;
        
        switch (linkOperator)
        {
            case Operators.LINK_OPERATOR_AND:
                predicate = AndAlso;
                break;
            case Operators.LINK_OPERATOR_OR:
                predicate = OrElse;
                break;
            default:
                throw new NotImplementedException();
        }

        foreach (SearchFilterModel searchFilterModel in filters.Where(o => o.Value != null))
        {
            Expression<Func<TEntity, bool>> thisExpression = searchFilterModel.CreateLambdaExpression<TEntity>();
            lambda = lambda == null ? 
                thisExpression : 
                predicate(lambda, thisExpression);
        }

        return lambda;
    }
}