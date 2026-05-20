using LMSModelLibrary.Models;

namespace LMSBLLibrary.Interfaces;

public interface IBookService
{
    void AddBook(Book book);
    void AddBookCopy(BookCopy bookCopy);
    List<Book> GetAllBooks();
    List<Book> SearchByTitle(string title);
    List<Book> SearchByAuthor(string author);
    void MarkBookCopyAsDamaged(int bookCopyId);
    List<Book> GetAvailableBooks();
    
}