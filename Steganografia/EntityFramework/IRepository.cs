using System.Linq;

namespace Steganografia.EntityFramework
{
    public interface IRepository<T>
    {
        IQueryable<T> AsQueryable();
        IQueryable<T> AsNoTracking();
        T Add(T entity);
        T Find(object id);
        void SaveOrUpdate();
    }
}