using LMSBLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Models;
using LMSModelLibrary.Enums;
using LMSDALLibrary.Repositories;
using LMSDALLibrary.Context;

namespace LMSBLLibrary.Services;

public class BorrowingService : IBorrowingService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IRepository<BookCopy> _bookCopyRepository;
    private readonly IFineRepository _fineRepository;
    private readonly LibraryDbContext _context;

    public BorrowingService(IMemberRepository memberRepository, IBookRepository bookRepository, IBorrowingRepository borrowingRepository, IFineRepository fineRepository, LibraryDbContext context, IRepository<BookCopy> bookCopyRepository)
    {
        _memberRepository = memberRepository;
        _bookRepository = bookRepository;
        _borrowingRepository = borrowingRepository;
        _fineRepository = fineRepository;
        _context = context;
        _bookCopyRepository = bookCopyRepository;
    }

    public void BorrowBook(int userId, int bookCopyId)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var member = _memberRepository.GetById(userId);
            var bookCopy = _bookCopyRepository.GetById(bookCopyId);

            if(member == null ||  !member.IsActive)
                throw new Exception("member not found or inactive");

            // to do : validate borrow limit based on membership, member borrowing a duplicate copy of the same book,
            // validate copy availability, then create a borrowing and update status of the book copy to borrowed.

            // Get active borrowings
            List<Borrowing> activeBorrowings = _borrowingRepository.GetActiveBorrowings(userId);
            // Now check if he has reached borrowing limit 
            if(activeBorrowings.Count >= member.MembershipType.MaxBorrowLimit) 
                throw new Exception("borrowing limit reached for this member");

            // check pending fine amount
            var finePending = _fineRepository.GetTotalUnpaidFine(userId); // stored procedure
            if(finePending > 500) 
                throw new Exception("pay fine first");

            // book copy not available
            if(bookCopy == null || bookCopy.Status != BookCopyStatus.Available)
                throw new Exception("book is not found or not available");

            // check duplicate borrowing
            bool borrowed = activeBorrowings.Any(b => b.BookCopy.BookId == bookCopy.BookId);
            if(borrowed) 
                throw new Exception("book already borrowed");

            Borrowing borrowing = new Borrowing{
                MemberId = userId, 
                Status = BorrowingStatus.Active, 
                BookCopyId = bookCopyId, 
                BorrowDate = DateTime.UtcNow, 
                DueDate = DateTime.UtcNow.AddDays(member.MembershipType.MaxBorrowDays)
                };
            _borrowingRepository.Add(borrowing);

            bookCopy.Status = BookCopyStatus.Borrowed;
            _bookCopyRepository.Update(bookCopy);
            // testing the transaction
            throw new Exception("rollback test");

            // transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

    }

    public List<(string Title, int Count)> GetMostBorrowedBooks()
    {
        return _borrowingRepository.GetMostBorrowedBooks();
    }

    public void ReturnBook(int borrowingId)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var borrowing = _borrowingRepository.GetById(borrowingId);
            if(borrowing == null) 
                throw new Exception("borrowing not found");
            
            // if book already returned
            if(borrowing.Status == BorrowingStatus.Returned)
                throw new Exception("book already returned");

            borrowing.ReturnDate = DateTime.UtcNow;
            borrowing.Status = BorrowingStatus.Returned;
            _borrowingRepository.Update(borrowing);
            // borrowing.BookCopy.Status = BookCopyStatus.Available;

            var copy = _bookCopyRepository.GetById(borrowing.BookCopyId);
            if(copy == null) 
                throw new Exception("book copy not found");
            
            copy.Status = BookCopyStatus.Available;
            _bookCopyRepository.Update(copy);
            
            Console.WriteLine($"Current Time: {DateTime.UtcNow}");
            Console.WriteLine($"Due Date: {borrowing.DueDate}");

            if(DateTime.UtcNow > borrowing.DueDate)
            {
                Console.WriteLine("Entered overdue block");

                int lateDays = (int)Math.Ceiling(
                    (DateTime.UtcNow - borrowing.DueDate).TotalDays
                );

                Console.WriteLine($"Late Days: {lateDays}");

                decimal fineAmount = lateDays * 10;

                Console.WriteLine($"Fine Amount: {fineAmount}");

                Fine fine = new Fine
                {
                    IsPaid = false,
                    Amount = fineAmount,
                    BorrowingId = borrowing.Id,
                };

                Console.WriteLine("Adding fine...");

                _fineRepository.Add(fine);

                Console.WriteLine("Fine added");
            }
            else
            {
                Console.WriteLine("NOT overdue");
            }

            transaction.Commit();
        }   
        catch
        {
            transaction.Rollback();
            throw;
        }
        
    }
}