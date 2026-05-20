using LMSModelLibrary.Models;
using LMSDALLibrary.Interfaces;
using LMSDALLibrary.Repositories;
using LMSDALLibrary.Context;
using Microsoft.EntityFrameworkCore;
using LMSModelLibrary.Enums;


namespace LMSDALLibrary.Repositories;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context)
        : base(context)
    {
    }
    public List<Book> SearchByTitle(string title)
    {
        return _dbSet.Where(book => book.Title.Contains(title)).ToList();
    }

    public List<Book> SearchByAuthor(string author)
    {
        return _dbSet.Where(book => book.Author.Contains(author)).ToList();
    }

    public List<Book> GetAvailableBooks()
    {
        return _dbSet.Include(b => b.BookCopies)
            .Where(b => b.BookCopies.Any(c => c.Status == BookCopyStatus.Available)).ToList();
    }
}