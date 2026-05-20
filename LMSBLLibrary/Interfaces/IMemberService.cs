using LMSModelLibrary.Models;

namespace LMSBLLibrary.Interfaces;

public interface IMemberService
{
    void AddMember(Member member);
    List<Member> GetAllMembers();
    Member? SearchByEmail(string email);
    Member? SearchByPhone(string phone);
    void DeactivateMember(int memberId);
    void ActivateMember(int memberId);
}
