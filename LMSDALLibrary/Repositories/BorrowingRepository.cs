using LMSDALLibrary.Context;
using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSDALLibrary.Repositories;

public class BorrowingRepository : Repository<Borrowing>, IBorrowingRepository
{
    public BorrowingRepository(LibraryDbContext context)
        : base(context)
    {
    }

    public override Borrowing? GetById(int id)
    {
        return _dbSet.Include(b => b.BookCopy)
            .Include(b => b.Fine)
            .FirstOrDefault(b => b.Id == id);
    }
    public List<Borrowing> GetActiveBorrowings(int id)
    {
        return _dbSet.Include(b => b.BookCopy)
            .Include(b => b.Fine)
            .Where(b => b.ReturnDate == null && b.MemberId == id)
            .ToList();
    }
    public List<Borrowing> GetBorrowingsByMember(int id)
    {
        return _dbSet.Include(b => b.Fine)
            .Include(b => b.BookCopy)
            .Include(b => b.Member)
            .Where(b => b.MemberId == id)
            .ToList();
    }
    public List<Borrowing> GetActiveBorrowings()
    {
        return _dbSet.Where(b => b.ReturnDate == null)
            .ToList();
    }
    public List<(string Title, int Count)> GetMostBorrowedBooks()
    {
        var borrowings = _dbSet
            .Include(b => b.BookCopy)
            .ThenInclude(c => c.Book)
            .ToList();

        return borrowings.GroupBy(b => b.BookCopy.Book.Title)
            .Select(g => (Title: g.Key, Count: g.Count()))
            .OrderByDescending(x => x.Count)
            .ToList();
    }
}