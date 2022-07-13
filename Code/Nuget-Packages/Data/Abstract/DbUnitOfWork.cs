using Fairway.Core.Data.Sql.Base.Interface;
using System;


namespace Fairway.Core.Data.Sql.EF
{
    public class DbUnitOfWork : IDbUnitOfWork
    {
        private readonly IDbContext _readonlyDbContext;
        private readonly IDbContext _readWriteDbContext;
        private readonly IServiceProvider _provider;
        

        public DbUnitOfWork(IDbContext readonlyDbContext, IDbContext readWriteDbContext, IServiceProvider provider)
        {
            _readonlyDbContext = readonlyDbContext;
            _readWriteDbContext = readWriteDbContext;
            _provider = provider;
        }

        /// <summary>
        /// Gets the lazy instance of the repository requested.
        /// </summary>
        /// <typeparam name="T">Type of repository requested</typeparam>
        /// <returns>Lazy instance of the repository requested</returns>
        public T GetRepository<T>() where T : IDbRepository
        {
            var repo = (T)_provider.GetService(typeof(T));
            if (repo.GetDbContext(true).GetType() != _readWriteDbContext.GetType())
                throw new Exception($"Requested repository's context type {repo.GetDbContext(true).GetType().Name} didn't match the context type {_readWriteDbContext.GetType().Name} of this unit of work");

            return repo;
        }

        /// <summary>
        /// Save all changes made as part of this unit of work.
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _readWriteDbContext.Save();
        }

        /// <summary>
        /// Rollback all changes made as part of this unit of work.
        /// </summary>
        /// <returns></returns>
        public void Rollback()
        {
            _readWriteDbContext.RollBack();
        }


    }
}
