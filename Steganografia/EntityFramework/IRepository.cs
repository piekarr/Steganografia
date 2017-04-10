using System.Linq;
using Steganografia.Models.Sessions;

namespace Steganografia.EntityFramework
{
    public interface IRepository<T>
    {
        IQueryable<T> AsQueryable();
        T Add(T entity);
    }
}