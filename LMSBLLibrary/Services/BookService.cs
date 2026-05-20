using LMSBLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;
using LMSModelLibrary.Enums;
using LMSDALLibrary.Context;

namespace LMSBLLibrary.Services;

public class BookService : IBookService
{
    private readonly IRepository<BookCopy> _bookCopyRepository;
    private readonly IBookRepository _bookRepository;
    private readonly LibraryDbContext _context;
    public BookService(IRepository<BookCopy> bookCopyRepository, IBookRepository bookRepository,LibraryDbContext context)
    {
        _bookCopyRepository = bookCopyRepository;
        _bookRepository = bookRepository;
        _context = context;
    }
    public void AddBook(Book book)
    {
        _bookRepository.Add(book);
    }
    public void AddBookCopy(BookCopy bookCopy)
    {
        _bookCopyRepository.Add(bookCopy);
    }
    public List<Book> SearchByAuthor(string author)
    {
        return _bookRepository.SearchByAuthor(author);
    }
    public List<Book> SearchByTitle(string title)
    {
        return _bookRepository.SearchByTitle(title);
    }
    public List<Book> GetAllBooks()
    {
        return _bookRepository.GetAll();
    }
    public void MarkBookCopyAsDamaged(int bookCopyId)
    {
        var copy = _context.BookCopies
            .FirstOrDefault(b => b.Id == bookCopyId);

        if(copy == null)
            throw new Exception("Book copy not found");

        copy.Status = BookCopyStatus.Damaged;

        _context.SaveChanges();
    }

    public List<Book> GetAvailableBooks()
    {
        return _bookRepository.GetAvailableBooks();
    }
}