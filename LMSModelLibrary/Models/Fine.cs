namespace LMSModelLibrary.Models;

public class Fine
{
    public int Id {get; set;}
    public decimal Amount {get; set;}
    public bool IsPaid {get; set;} = false;
    public DateTime? PaidDate {get; set;}

    // Foreign Key
    public int BorrowingId {get; set;}

    // Navigation Property Borrowing 1....1 Fine
    public Borrowing Borrowing {get; set;}
}