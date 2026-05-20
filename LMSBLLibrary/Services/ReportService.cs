using LMSBLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;

namespace LMSBLLibrary.Services;

public class ReportService: IReportService
{
    private readonly IFineRepository _fineRepository;
    private readonly IBorrowingRepository _borrowingRepository;
    public ReportService(IFineRepository fineRepository, IBorrowingRepository borrowingRepository)
    {
        _borrowingRepository = borrowingRepository;
        _fineRepository = fineRepository;
    }

    public List<Borrowing> GetActiveBorrowings()
    {
        return _borrowingRepository.GetActiveBorrowings();
    } 
    public List<Borrowing> GetOverdueBorrowings()
    {
        return _borrowingRepository.GetActiveBorrowings()
                                   .Where(b => b.DueDate < DateTime.UtcNow)
                                   .ToList();
    }
    public List<Fine> GetPendingFines()
    {
        return _fineRepository.GetPendingFines();
    }
    public List<Borrowing> GetBorrowingHistory(int memberId)
    {
        return _borrowingRepository.GetBorrowingsByMember(memberId);
    }
}