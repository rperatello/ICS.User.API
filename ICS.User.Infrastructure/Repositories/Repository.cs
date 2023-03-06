using ICS.User.Domain.Interfaces;
using ICS.User.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ICS.User.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected ICSDbContext _context;

    public Repository(ICSDbContext context)
    {
        _context = context;
    }

    public virtual IQueryable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking();        
    }

    public virtual async Task<T?> Where(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
    }

    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public virtual void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.Set<T>().Update(entity);
    }

    public virtual void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }    
}