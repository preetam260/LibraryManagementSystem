namespace LMSModelLibrary.Models;

public class BookCategory
{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    // Navigation property BookCategor 1....N Books
    public ICollection<Book> Books {get; set;} = new List<Book>();
}