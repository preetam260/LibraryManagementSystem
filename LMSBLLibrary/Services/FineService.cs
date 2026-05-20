using LMSBLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;

namespace LMSBLLibrary.Services;

public class FineService : IFineService
{
    private readonly IFineRepository _fineRepository;
    private readonly IBorrowingRepository _borrowingRepository; // service layer should depend on interface not directly on repo
    public FineService(IFineRepository fineRepository, IBorrowingRepository borrowingRepository)
    {
        _fineRepository = fineRepository;
        _borrowingRepository = borrowingRepository;
    }

    public List<Fine> GetPendingFines(int memberId)
    {
        return _borrowingRepository
            .GetBorrowingsByMember(memberId)
            .Where(b => b.Fine != null && !b.Fine.IsPaid)
            .Select(b => b.Fine!)
            .ToList();
    }

    // Get all fine history for a member
    public List<Fine> GetFineHistory(int memberId)
    {
        return _borrowingRepository
            .GetBorrowingsByMember(memberId)
            .Where(b => b.Fine != null)
            .Select(b => b.Fine!)
            .ToList();
    }

    // Pay a fine
    public void PayFine(int fineId)
    {
        var fine = _fineRepository.GetById(fineId);

        if (fine == null)
            throw new Exception("Fine not found");
        
        if (fine.IsPaid)
            throw new Exception("Fine already paid");
        fine.IsPaid = true;

        fine.PaidDate = DateTime.UtcNow;

        _fineRepository.Update(fine);
    }


}