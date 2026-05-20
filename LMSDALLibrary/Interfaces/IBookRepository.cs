using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    List<Book> SearchByTitle(string title);
    List<Book> SearchByAuthor(string author);
    List<Book> GetAvailableBooks();
}