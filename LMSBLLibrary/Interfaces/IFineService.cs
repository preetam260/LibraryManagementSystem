using LMSModelLibrary.Models;

namespace LMSBLLibrary.Interfaces;

public interface IFineService
{
    List<Fine> GetPendingFines(int memberId);
    void PayFine(int fineId);
    List<Fine> GetFineHistory(int memberId);
}