using System;
using System.Collections.Generic;
using System.Text;

namespace Fairway.Core.Data.Sql.Base.Interface
{
    public interface IDbUnitOfWork
    {        
        T GetRepository<T>() where T : IDbRepository;
        int SaveChanges();
        void Rollback();
    }
}
