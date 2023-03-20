using System.Linq.Expressions;

namespace ICS.User.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll(bool withTracking = false);
    Task<T?> Where(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
