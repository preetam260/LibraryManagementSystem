using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces;
public interface IMemberRepository: IRepository<Member>
{
    Member? GetByEmail(string email);
    Member? GetByPhone(string phone);
}