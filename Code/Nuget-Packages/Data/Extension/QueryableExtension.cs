using Fairway.Core.Data.Sql.Base.Models;
using Microsoft.EntityFrameworkCore;
using System;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Fairway.Core.Data.Sql.EF.Extension
{
    public static class QueryableExtension
    {
        public static IQueryable<T> IncludeTable<T>(this IQueryable<T> query, Expression<Func<T, object>> selectorPredicate) where T : DbEntityBase
        {
            return query.Include(selectorPredicate);
        }
    }
}
