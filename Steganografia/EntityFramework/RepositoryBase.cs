using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steganografia.EntityFramework
{
	public class RepositoryBase<T> : IRepository<T> where T : class
	{
		AppContext _appContext;
		private static object _l = new object();
		public RepositoryBase()
		{
			_appContext = AppContext.Create();
		}

		public T Create()
		{
			return _appContext.Set<T>().Create();
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

		public IQueryable<T> AsNoTracking()
		{
			var entitySet = _appContext.Set<T>();
			return entitySet.AsNoTracking();
		}

		public T Find(object id)
		{
			var entitySet = _appContext.Set<T>();
			return entitySet.Find(id);
		}

		public void SaveOrUpdate()
		{
			lock (_l)
			{
				_appContext.SaveChanges();

			}
		}

	}
}