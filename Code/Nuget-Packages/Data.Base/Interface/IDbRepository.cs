using Fairway.Core.Data.Sql.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Fairway.Core.Data.Sql.Base.Interface
{
    public interface IDbRepository
    {
        IDbContext GetDbContext(bool toUpdate = false);
    }

    public interface IDbRepository<T> : IDbRepository
        where T : IDbEntity, new()
    {
        DateTime GetDbCurrentDateTime();
        
        T GetById(object entityId, bool toUpdate = false);
        
        IEnumerable<T> Get(Expression<Func<T, bool>> wherePredicate,
            IEnumerable<string> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            PaginationSetting paginationSetting = null,
            bool toUpdate = false);

        T CreateNewOrGetExisting(object entityId);

        T Create();

        T Attach(T entity);

        void Delete(object entityId);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> wherePredicate);

        IEnumerable<T> Delete(IEnumerable<T> entities);

        //int Save();

        bool IsEntityLoaded<TParent>(DbEntityBase parentEntity, string navigationalPropertyName)
            where TParent : DbEntityBase;
        IEnumerable<TQueryResult> ExecuteSqlQuery<TQueryResult>(string command, params object[] parameters) where TQueryResult : DbEntityBase;

        int ExecuteSqlCommand(string command, params object[] parameters) ;
    }
}
