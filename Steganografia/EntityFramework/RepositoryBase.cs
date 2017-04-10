using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steganografia.EntityFramework
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        AppContext _appContext;

        public RepositoryBase()
        {
            _appContext = new AppContext();
        }

        public T Add(T entity)
        {
            var entitySet = _appContext.Set<T>();
            entitySet.Add(entity);
            _appContext.SaveChanges();
            return entity;
        }

        public IQueryable<T> AsQueryable()
        {
            var entitySet = _appContext.Set<T>();
            return entitySet.AsQueryable();
        }
    }
}