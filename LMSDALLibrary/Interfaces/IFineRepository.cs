using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces;

public interface IFineRepository : IRepository<Fine>
{
    List<Fine> GetPendingFines();
    decimal GetTotalUnpaidFine(int memberId);
}