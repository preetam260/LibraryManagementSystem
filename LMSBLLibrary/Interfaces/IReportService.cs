using LMSModelLibrary.Models;

namespace LMSBLLibrary.Interfaces;

public interface IReportService
{
    List<Borrowing> GetActiveBorrowings();
    List<Borrowing> GetOverdueBorrowings();
    List<Fine> GetPendingFines();
    List<Borrowing> GetBorrowingHistory(int memberId);
}