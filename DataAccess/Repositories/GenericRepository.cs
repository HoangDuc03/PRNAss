using BusinessObject.Models;
using DataAccess.Repositories;
using System.Linq.Expressions;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private static GenericRepository<T>? _instance;
    private static readonly object _lock = new object();
    private readonly EStoreContext _context;

    public GenericRepository(EStoreContext context)
    {
        _context = context;
    }

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
        return entity;
    }

    public T GetId(int id)
    {
        return _context.Set<T>().Find(id);
    }
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).ToList();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void Remove(int id)
    {
        _context.Remove(GetId(id));
        _context.SaveChanges();
    }
}
