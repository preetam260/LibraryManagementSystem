namespace LMSModelLibrary.Models;

public class MembershipType
{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public int MaxBorrowLimit {get; set;}
    public int MaxBorrowDays {get; set;}
    
    // Navigation property MembershipType 1....N Member
    public ICollection<Member> Members {get; set;} = new List<Member>();
}