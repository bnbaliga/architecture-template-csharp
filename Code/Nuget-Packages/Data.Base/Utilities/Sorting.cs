using Fairway.Core.Data.Sql.Base.Enums;
using Fairway.Core.Data.Sql.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fairway.Core.Data.Sql.Base.Utilities
{
    public static class Sorting
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, IEnumerable<OrderBySetting> orderBySetting) where T : class
        {
            var isQueryOrdered = false;
            foreach (var setting in orderBySetting)
            {
                var orderedQuery = query.ApplyOrder<T>(setting, isQueryOrdered);

                if (orderedQuery == null) continue;

                query = orderedQuery;
                isQueryOrdered = true;
            }

            return query;
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> query, OrderBySetting orderBySetting, bool isQueryOrdered) where T : class
        {
            if (orderBySetting != null)
            {
                var returnType = typeof(T);
                var orderOnType = returnType;

                var orderByEntity = (string.IsNullOrWhiteSpace(orderBySetting.TableName) ? returnType.Name : orderBySetting.TableName);
                var orderByColumn = orderBySetting.ColumnName;
                var orderByEntityNavigationRoute = (string.IsNullOrWhiteSpace(orderBySetting.NavigationRoute) ? string.Empty : orderBySetting.NavigationRoute + ".") + orderByEntity + "." + orderByColumn;
                var orderByDirection = orderBySetting.Direction;

                if (!string.Equals(returnType.Name, orderByEntity, StringComparison.CurrentCultureIgnoreCase))
                    if (returnType.AssemblyQualifiedName != null)
                        orderOnType = Type.GetType(returnType.AssemblyQualifiedName.Replace(returnType.Name, orderByEntity));

                if (orderOnType == null) return null;

                string orderByMethodName;
                if (isQueryOrdered)
                    orderByMethodName = (orderByDirection == OrderByDirection.Desc ? "ThenByDescending" : "ThenBy");
                else
                    orderByMethodName = (orderByDirection == OrderByDirection.Desc ? "OrderByDescending" : "OrderBy");

                var orderOnDataType = returnType;
                var parameterExpression = Expression.Parameter(returnType);
                var property = (Expression)parameterExpression;
                var props = orderByEntityNavigationRoute.Split('.');
                foreach (var prop in props.Skip(1))
                {
                    var pi = orderOnDataType.GetProperty(prop);
                    property = Expression.Property(property, pi);
                    orderOnDataType = pi.PropertyType;
                }
                var delegateType = typeof(Func<,>).MakeGenericType(returnType, orderOnDataType);
                var lambda = Expression.Lambda(delegateType, property, parameterExpression);

                var methodInfo = typeof(Queryable).GetMethods().Single(
                    m => m.Name == orderByMethodName
                              && m.IsGenericMethodDefinition
                              && m.GetGenericArguments().Length == 2
                              && m.GetParameters().Length == 2)
                              .MakeGenericMethod(returnType, orderOnDataType);

                return (IOrderedQueryable<T>)methodInfo.Invoke(null, new object[] { query, lambda });
            }

            return null;
        }
    }
}
