using Fairway.Core.Data.Sql.Base.Attributes;
using Fairway.Core.Data.Sql.Base.Enums;
using Fairway.Core.Data.Sql.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fairway.Core.Data.Sql.Base.Utilities
{
    public static class Pagination
    {
        public static PaginationSetting CreatePagination(int pageNumber, int pageSize, IEnumerable<OrderBySetting> orderBySettings)
        {
            return new PaginationSetting(pageNumber, pageSize, orderBySettings);
        }

        public static IEnumerable<OrderBySetting> GetOrderBySettings(Type dtoType, string sort = "")
        {
            var result = new List<OrderBySetting>();

            if (string.IsNullOrWhiteSpace(sort))
                result.AddRange(GetDefaultOrderBySettings(dtoType));
            else
            {
                foreach (var orderBy in sort.Split(','))
                {
                    var firstChar = orderBy.Substring(0, 1);
                    var orderByDirection = OrderByDirection.Asc;
                    if (firstChar == "-")
                        orderByDirection = OrderByDirection.Desc;

                    string orderByColumn;
                    if (firstChar == "+" || firstChar == "-")
                        orderByColumn = orderBy.Substring(1, orderBy.Length - 1);
                    else
                        orderByColumn = orderBy;

                    if (string.IsNullOrWhiteSpace(orderByColumn))
                        throw new ArgumentException($"Invalid column '{orderBy}'");

                    var propertyInfo = dtoType.GetProperty(orderByColumn,
                        BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (propertyInfo == null)
                        throw new ArgumentException($"Column '{orderByColumn}' does not exist");

                    result.Add(GetOrderBySetting(propertyInfo, orderByDirection));
                };
            }

            return result.ToArray();
        }

        private static IEnumerable<OrderBySetting> GetDefaultOrderBySettings(Type dtoType)
        {
            foreach (var propertyInfo in dtoType.GetProperties().Where(w => w.GetCustomAttributes(typeof(OrderByAttribute), false).FirstOrDefault() != null).ToList())
            {
                var isDefaultArgument = GetPropertyAttribute(propertyInfo, "OrderBy", "IsDefault");
                var isDefault = isDefaultArgument?.ToString().BooleanParse() ?? false;

                if (!isDefault) continue;

                var defaultOrderArgument = GetPropertyAttribute(propertyInfo, "OrderBy", "DefaultOrder");
                var defaultOrder = OrderByDirection.Asc;
                if (defaultOrderArgument != null)
                    defaultOrder = (OrderByDirection)int.Parse(defaultOrderArgument.ToString());

                yield return GetOrderBySetting(propertyInfo, defaultOrder);
            }
        }

        private static OrderBySetting GetOrderBySetting(PropertyInfo propertyInfo, OrderByDirection orderByDirection)
        {
            var tableName = GetPropertyAttribute(propertyInfo, "OrderBy", "TableName")?.ToString();
            var columnName = GetPropertyAttribute(propertyInfo, "OrderBy", "ColumnName")?.ToString();
            var navigationRoute = GetPropertyAttribute(propertyInfo, "OrderBy", "NavigationRoute")?.ToString();

            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException($"Column '{propertyInfo.Name}' is not setup for sorting purposes");

            return new OrderBySetting()
            {
                TableName = tableName,
                ColumnName = columnName,
                NavigationRoute = navigationRoute,
                Direction = orderByDirection
            };
        }

        public static IQueryable<T> Paginate<T>(this IOrderedQueryable<T> query, PaginationSetting paginationSetting) where T : class
        {
            return PaginateMe<T>(query, paginationSetting);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationSetting paginationSetting) where T : class
        {
            return PaginateMe<T>(query, paginationSetting);
        }

        private static IQueryable<T> PaginateMe<T>(IQueryable<T> query, PaginationSetting paginationSetting) where T : class
        {
            if (paginationSetting == null) return query;

            paginationSetting.RecordCount = query.AsQueryable<T>().Count();
            if (paginationSetting.PageNumber > paginationSetting.PageCount)
                if (paginationSetting.PageCount != null)
                    paginationSetting.PageNumber = paginationSetting.PageCount.Value;

            query = query.Sort(paginationSetting.OrderBySettings);
            query = query.Skip((paginationSetting.PageNumber - 1) * paginationSetting.PageSize).Take(paginationSetting.PageSize);

            return query;
        }

        private static object GetPropertyAttribute(PropertyInfo propertyInfo, string attributeName, string argumentName)
        {
            // look for an attribute that takes one constructor argument
            var attribute = propertyInfo.GetCustomAttributesData()
                .FirstOrDefault(w => w.Constructor.DeclaringType?.Name.Replace("Attribute", string.Empty) == attributeName);

            return attribute?.NamedArguments?.FirstOrDefault(w => w.MemberName == argumentName).TypedValue.Value;
        }

        public static bool BooleanParse(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return false;

            value = value.ToLower();
            return value.Equals("1") || value.Equals("yes") || value.Equals("true");
        }
    }
}
