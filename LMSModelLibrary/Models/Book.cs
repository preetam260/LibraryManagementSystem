namespace LMSModelLibrary.Models;

public class Book
{
    public int Id {get; set;}
    public string Title {get; set;} = "";
    public string Author {get; set;}
    
    // Foreign Key
    public int CategoryId {get; set;}

    // Navigation Property Book 1...N BookCopy, Book 1....1BookCategory
    public BookCategory Category {get; set;}
    public ICollection<BookCopy> BookCopies {get; set;} = new List<BookCopy>();

}