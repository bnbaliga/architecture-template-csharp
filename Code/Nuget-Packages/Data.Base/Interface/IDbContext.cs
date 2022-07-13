using Fairway.Core.Data.Sql.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fairway.Core.Data.Sql.Base.Interface
{
    public interface IDbContext
    {
        DateTime GetDbCurrentDateTime();

        IQueryable<T> Get<T>(Expression<Func<T, bool>> wherePredicate,
            IEnumerable<string> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
           ) where T : DbEntityBase;

        T Find<T>(object id) where T : DbEntityBase;

        T Create<T>() where T : DbEntityBase, new();

        T AttachToContext<T>(T entity) where T : DbEntityBase;

        void Delete<T>(T entity) where T : DbEntityBase;

        void Delete<T>(Expression<Func<T, bool>> wherePredicate) where T : DbEntityBase;

        IEnumerable<T> Delete<T>(IEnumerable<T> entities) where T : DbEntityBase;

        int Save();

        void RollBack();       

        int ExecuteSqlCommand(string functionStoredProcName, params Object[] parameters);        

        IQueryable<T> ExecuteQuery<T>(string query, params Object[] parameters) where T : DbEntityBase;

        bool IsEntityLoaded<TParent>(DbEntityBase parentEntity, string navigationalPropertyName)
            where TParent : DbEntityBase;
    }
}
