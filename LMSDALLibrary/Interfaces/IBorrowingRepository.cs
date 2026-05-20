using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces;

public interface IBorrowingRepository : IRepository<Borrowing>
{
    List<Borrowing> GetActiveBorrowings(int memberId);
    List<Borrowing> GetBorrowingsByMember(int memberId);
    List<Borrowing> GetActiveBorrowings();
    List<(string Title, int Count)> GetMostBorrowedBooks();
}