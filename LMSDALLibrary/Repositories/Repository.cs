using LMSDALLibrary.Context;
using LMSDALLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSDALLibrary.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly LibraryDbContext _context; // connection manager
    protected readonly DbSet<T> _dbSet; // table manager
    public Repository(LibraryDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public virtual T? GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public void Add(T item)
    {
        _context.Set<T>().Add(item);
        _context.SaveChanges();
    }

    public void Update(T item)
    {
        _context.Set<T>().Update(item);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = GetById(id);
        if (item != null)
        {
            _context.Set<T>().Remove(item);
            _context.SaveChanges();
        }
    }

}
