using LMSModelLibrary.Enums;

namespace LMSModelLibrary.Models;

public class BookCopy
{
    public int Id {get; set;}
    public BookCopyStatus Status {get; set;} = BookCopyStatus.Available;

    // Foreign Key
    public int BookId {get; set;}

    // Navigation Properties BookCopy 1....1 Book, BookCopy 1....N Borrowings
    public Book Book {get; set;} = null!;
    public ICollection<Borrowing> Borrowings {get; set;} = new List<Borrowing>();
}