using Microsoft.EntityFrameworkCore;

namespace OldSchool.Infrastructure;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationContext _context;

    public Repository(ApplicationContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>();
    }

    public ValueTask<T?> GetByIdAsync(int id)
    {
        return _context.FindAsync<T>(id);
    }

    public void Create(T item)
    {
        _context.Add(item);
    }

    public void Update(T item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.FindAsync<T>(id);
        if (item != null)
            _context.Set<T>().Remove(item);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}