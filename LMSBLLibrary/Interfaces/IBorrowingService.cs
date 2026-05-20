namespace LMSBLLibrary.Interfaces;

public interface IBorrowingService
{
    void BorrowBook(int memberId, int bookCopyId);
    void ReturnBook(int borrowingId);
    List<(string Title, int Count)> GetMostBorrowedBooks();
}