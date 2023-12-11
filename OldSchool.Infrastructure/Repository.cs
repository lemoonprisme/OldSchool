using Microsoft.EntityFrameworkCore;

namespace OldSchool.Infrastructure;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationContext _context;

    public Repository(ApplicationContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>();
    }

    public T? GetById(int id)
    {
        return _context.Find<T>(id);
    }

    public void Create(T item)
    {
        _context.Add(item);
    }

    public void Update(T item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
        var item = _context.Find<T>(id);
        if (item != null)
            _context.Set<T>().Remove(item);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}