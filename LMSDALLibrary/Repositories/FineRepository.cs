using LMSDALLibrary.Context;
using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSDALLibrary.Repositories;

public class FineRepository
    : Repository<Fine>, IFineRepository
{
    public FineRepository(LibraryDbContext context)
        : base(context)
    {
    }

    public List<Fine> GetPendingFines()
    {
        return _dbSet.Include(f => f.Borrowing)
            .Where(f => !f.IsPaid)
            .ToList();
    }
    public decimal GetTotalUnpaidFine(int memberId)
    {
        var result = _context.Database.SqlQuery<decimal>(
                $"SELECT calculate_member_fine({memberId})")
            .AsEnumerable()
            .FirstOrDefault();

        return result;
    }
}