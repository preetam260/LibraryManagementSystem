namespace LMSModelLibrary.Models;

public class Member
{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Email {get; set;} = "";
    public string Phone {get; set;} = "";
    public bool IsActive {get; set;} = true;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    
    // Foreign Key references MembershipType(Id)
    public int MembershipTypeId {get; set;}

    // Navigation Property Member 1....1 MembershipType, Member 1....N Borrowing
    public MembershipType MembershipType {get; set;}
    public ICollection<Borrowing> Borrowings {get; set;}= new List<Borrowing>();
    // Fines is not required as it is an indirect navigation. Can still add it but not
    // necessary. If you add it then you might need to add a MemberId(direct relation mapping)
    // inside Fine.cs -> Redundancy
}