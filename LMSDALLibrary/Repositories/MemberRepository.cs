using LMSModelLibrary.Models;   
using LMSDALLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using LMSDALLibrary.Context;

namespace LMSDALLibrary.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(LibraryDbContext context)
        : base(context)
    {
    }
    public override Member? GetById(int id)
    {
        return _context.Members.Include(m => m.MembershipType)
            .FirstOrDefault(m => m.Id == id);
    }
    public Member? GetByEmail(string email)
    {
        return _dbSet.FirstOrDefault(m => m.Email == email);
    }

    public Member? GetByPhone(string phone)
    {
        return _dbSet.FirstOrDefault(m => m.Phone == phone);
    }
}