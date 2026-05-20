using LMSBLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;

namespace LMSBLLibrary.Services;

public class MemberService : IMemberService
{   
    private readonly IMemberRepository _memberRepository;
    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public void AddMember(Member member)
    {
        _memberRepository.Add(member);
    }
    public List<Member> GetAllMembers()
    {
        return _memberRepository.GetAll();
    }
    public Member? SearchByEmail(string email)
    {
        return _memberRepository.GetByEmail(email);
    }
    public Member? SearchByPhone(string phone)
    {
        return _memberRepository.GetByPhone(phone);
    }
    public void DeactivateMember(int memberId)
    {
        var member = _memberRepository.GetById(memberId);

        if(member != null)
        {
            member.IsActive = false;
            _memberRepository.Update(member);
        }
        else
        {
            throw new Exception("member not found");
        }
    }

    public void ActivateMember(int memberId)
    {
        var member = _memberRepository.GetById(memberId);

        if(member != null && member.IsActive == false)
        {
            member.IsActive = true;
            _memberRepository.Update(member);
        }
        else
        {
            throw new Exception("member not found");
        }
    }

}