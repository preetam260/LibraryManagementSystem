using LMSModelLibrary.Enums;

namespace LMSModelLibrary.Models;

public class Borrowing
{
    public int Id {get; set;}
    public DateTime BorrowDate {get; set;} = DateTime.UtcNow;
    public DateTime DueDate {get; set;}
    public DateTime? ReturnDate {get; set;}
    public BorrowingStatus Status {get; set;} = BorrowingStatus.Active;

    // Foreign Keys
    public int MemberId {get; set;}
    public int BookCopyId {get; set;}

    // Navigation Properties 
    public Member Member {get; set;} = null; // Borrowing belongs to 1 member 
    public BookCopy BookCopy {get; set;} = null; // Borrowing associated with 1 physical BookCopy
    public Fine? Fine {get; set;} // Borrowing can have a fine or not 0/1
}