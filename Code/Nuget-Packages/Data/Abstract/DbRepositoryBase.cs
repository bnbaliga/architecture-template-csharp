using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fairway.Core.Data.Sql.Base.Interface;
using Fairway.Core.Data.Sql.Base.Models;
using Fairway.Core.Data.Sql.Base.Utilities;

namespace Fairway.Core.Data.Sql.EF
{
    public abstract class DbRepositoryBase<T> : IDbRepository<T>
        where T : DbEntityBase, new()
    {
        private readonly IDbContext _readonlyDbContext;
        private readonly IDbContext _readWriteDbContext;       
   

        protected DbRepositoryBase(IDbContext readonlyDbContext, IDbContext readWriteDbContext)
        {            
            _readonlyDbContext = readonlyDbContext;
            _readWriteDbContext = readWriteDbContext;
        }

        /// <summary>
        /// Returns the DB Context instance from the repo base
        /// </summary>
        /// <param name="toUpdate"> Get context for either read only usage or for making modifications. 
        /// In read only mode this will disable tracking for the entiries retrieved.</param>
        /// <returns></returns>
        public virtual IDbContext GetDbContext(bool toUpdate = false)
        {
            if (toUpdate)
                return _readWriteDbContext;
            return _readonlyDbContext;
        }

        public DateTime GetDbCurrentDateTime()
        {
            return GetDbContext().GetDbCurrentDateTime();
        }

        /// <summary>
        /// Get entity by primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Particular instance of specified type. If entity is not found it will return null.</returns>
        public virtual T GetById(object id, bool toUpdate = false)
        {
            return GetDbContext(toUpdate).Find<T>(id);
        }


        /// <summary>
        /// Get a filtered list of entities of given type.
        /// </summary>
        /// <param name="wherePredicate">Where clause to filter the entities.</param>
        /// <param name="orderBy">Optionally orders the data based on the order By clause.</param>
        /// <param name="includeProperties">Optionally includes the dependent objects. Use comma seperated string to include multiple types for eager loading.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> wherePredicate,
            IEnumerable<string> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            PaginationSetting paginationSetting = null,
            bool toUpdate = false)
        {
            return GetDbContext(toUpdate)
                .Get<T>(wherePredicate, includes, orderBy)                
                .Paginate(paginationSetting)
                .ToList();
        }

        /// <summary>
        /// Implemnt custom logic if any that needs to be executed right after creating a new instance and adding to context.
        /// </summary> 
        /// /// <param name="entity">newly created entity reference.</param>
        protected virtual void OnEntityInserting(IDbEntity entity)
        {
            //Base implementation doesn't do anything. Provided for implemnting any common logic right after insertion.   
        }

        public T CreateNewOrGetExisting(object id)
        {
            T entity;
            if (id == null)
                entity = Create();
            else
                entity = GetById(id, true) ?? Create();
            
            return entity;
        }

        /// <summary>
        /// Created and adds a new entity into context. Save has to be called after this to persist data to store/database.
        /// </summary>        
        public T Create()
        {
            var entity = GetDbContext(true).Create<T>();
            OnEntityInserting(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            return GetDbContext(true).AttachToContext<T>(entity);
        }

        /// <summary>
        /// Deletes the entity based on the identifier.
        /// </summary>
        /// <param name="id">Unique identifier of the entity to be deleted.</param>
        public void Delete(object id)
        {           
            GetDbContext(true).Delete<T>(GetById(id, true));
        }

        /// <summary>
        /// Deleted the entity based on the entity instance.
        /// </summary>
        /// <param name="entityToDelete">Entity instance to be deleted.</param>
        public void Delete(T entity)
        {
            GetDbContext(true).Delete<T>(entity);
        }

        /// <summary>
        /// Deleted the list of entities.
        /// </summary>
        /// <param name="entities">List of entities to be deleted.</param>
        public IEnumerable<T> Delete(IEnumerable<T> entities)
        {
            return GetDbContext(true).Delete(entities);
        }


        /// <summary>
        /// Deleted the list of entities.
        /// </summary>
        /// <param name="wherePredicate">Where clause to filter the entities to be deleted.</param>
        public void Delete(Expression<Func<T, bool>> wherePredicate)
        {
            GetDbContext(true).Delete(wherePredicate);
        }

        public bool IsEntityLoaded<TParent>(DbEntityBase parentEntity, string navigationalPropertyName) where TParent : DbEntityBase
        {
            return GetDbContext(true).IsEntityLoaded<TParent>(parentEntity, navigationalPropertyName);
        }

        public int ExecuteSqlCommand(string command, params Object[] parameters)
        {
            return GetDbContext().ExecuteSqlCommand(command,parameters);
        }

        public IEnumerable<TQueryResult> ExecuteSqlQuery<TQueryResult>(string command, params Object[] parameters) where TQueryResult : DbEntityBase
        {
            return GetDbContext().ExecuteQuery<TQueryResult>(command, parameters).ToList();
        }

    }
}
