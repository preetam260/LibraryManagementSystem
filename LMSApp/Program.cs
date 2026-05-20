using LMSBLLibrary.Interfaces;
using LMSBLLibrary.Services;

using LMSDALLibrary.Context;
using LMSDALLibrary.Interfaces;
using LMSDALLibrary.Repositories;

using LMSModelLibrary.Enums;
using LMSModelLibrary.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)

    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseNpgsql(
                "Host=localhost;Port=5432;Database=lmsdb;Username=murasaki;Password=123456"));

        // Generic Repository
        services.AddScoped(typeof(IRepository<>),
            typeof(Repository<>));

        // Repositories
        services.AddScoped<IMemberRepository,
            MemberRepository>();

        services.AddScoped<IBookRepository,
            BookRepository>();

        services.AddScoped<IBorrowingRepository,
            BorrowingRepository>();

        services.AddScoped<IFineRepository,
            FineRepository>();

        // Services
        services.AddScoped<IMemberService,
            MemberService>();

        services.AddScoped<IBookService,
            BookService>();

        services.AddScoped<IBorrowingService,
            BorrowingService>();

        services.AddScoped<IFineService,
            FineService>();

        services.AddScoped<IReportService,
            ReportService>();
    })

    .Build();

Console.WriteLine("===== LMS APPLICATION =====");

using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

var memberService = services.GetRequiredService<IMemberService>();
var bookService = services.GetRequiredService<IBookService>();
var borrowingService = services.GetRequiredService<IBorrowingService>();
var fineService = services.GetRequiredService<IFineService>();
var reportService = services.GetRequiredService<IReportService>();

bool running = true;

while(running)
{
    Console.WriteLine("\n===== MAIN MENU =====");
    Console.WriteLine("Member Management(15 also)");
    Console.WriteLine("1. Add Member");
    Console.WriteLine("2. View Members");
    Console.WriteLine("3. Search Member By Email");
    Console.WriteLine("4. Search By Phone Number");
    Console.WriteLine("5. Deactivate Member");
    Console.WriteLine("6. Activate Member");
    
    Console.WriteLine("\n======================");
    Console.WriteLine("Book Management");
    Console.WriteLine("7. Add Book");
    Console.WriteLine("8. View Books");
    Console.WriteLine("9. Search Book By Title");
    Console.WriteLine("10. Search Book By Title");
    Console.WriteLine("11. Add Book Copy");
    Console.WriteLine("12. Mark Book Copy Damaged");

    Console.WriteLine("\n======================");
    Console.WriteLine("13. Borrow Book");
    Console.WriteLine("14. Return Book");

    Console.WriteLine("Fine Management");
    Console.WriteLine("15. View Pending Fines");

    Console.WriteLine("Reports");
    Console.WriteLine("16. Active Borrowings");
    Console.WriteLine("17. Borrowing History");
    Console.WriteLine("28. Most Borrowed Books");

    Console.WriteLine("\n======================");
    Console.WriteLine("19. Exit");

    Console.Write("\nEnter Choice: ");

    var choice = Console.ReadLine();

    try
    {
        switch(choice)
        {
            case "1":
                Console.Write("enter name ");
                string name = Console.ReadLine()!;

                Console.Write("enter email: ");
                string email = Console.ReadLine()!;

                Console.Write("enter phone number: ");
                string phone = Console.ReadLine()!;

                Console.Write("enter membership id type(0-basic, 1-student, 2-premium): ");
                int membershipTypeId =
                    int.Parse(Console.ReadLine()!);

                var member = new Member
                                {
                                    Name = name,
                                    Email = email,
                                    Phone = phone,
                                    MembershipTypeId = membershipTypeId
                                };

                memberService.AddMember(member);
                Console.WriteLine("Member Added");

                break;

            case "2":
                var members = memberService.GetAllMembers();

                foreach(var m in members)
                    Console.WriteLine($"{m.Id} | {m.Name} | {m.Email} | {m.Phone}");
                
                break;

            case "3":
                Console.Write("Enter Email: ");
                string searchEmail = Console.ReadLine()!;

                var searchedMember = memberService.SearchByEmail(searchEmail);

                if(searchedMember == null) 
                {
                    Console.WriteLine("Member Not Found");
                }
                else
                {
                    Console.WriteLine(
                        $"{searchedMember.Id} | {searchedMember.Name}");
                }

                break;
            
            case "4":
                Console.Write("Enter Phone Number: ");
                string searchNumber = Console.ReadLine()!;

                var searchedPMember = memberService.SearchByPhone(searchNumber);

                if(searchedPMember == null)
                {
                    Console.WriteLine("Member Not Found");
                }
                else
                {
                    Console.WriteLine(
                        $"{searchedPMember.Id} | {searchedPMember.Name}");
                }

                break;

            case "5":
                Console.Write("Member Id: ");
                int deactivateId = int.Parse(Console.ReadLine()!);

                memberService.DeactivateMember(deactivateId);
                Console.WriteLine("Member Deactivated");

                break;

            case "6":
                Console.Write("Member Id: ");
                int activateId = int.Parse(Console.ReadLine()!);

                memberService.ActivateMember(activateId);
                Console.WriteLine("Member Reactivated");

                break;

            case "7":
                Console.Write("Title: ");
                string title = Console.ReadLine()!;

                Console.Write("Author: ");
                string author = Console.ReadLine()!;

                Console.Write("Category Id: ");
                int categoryId = int.Parse(Console.ReadLine()!);

                var book = new Book
                            {
                                Title = title,
                                Author = author,
                                CategoryId = categoryId
                            };

                bookService.AddBook(book);
                Console.WriteLine("Book Added");

                break;

            case "8":
                var books = bookService.GetAllBooks();

                foreach(var b in books)
                {
                    Console.WriteLine($"{b.Id} | {b.Title} | {b.Author}");
                }

                break;

            case "9":
                Console.Write("Enter Author's Name: ");
                string searchName = Console.ReadLine()!;

                var searchedBo0ks = bookService.SearchByAuthor(searchName);

                foreach(var b in searchedBo0ks)
                {
                    Console.WriteLine($"{b.Id} | {b.Title}");
                }

                break;

            case "10":
                Console.Write("Enter Title: ");
                string searchTitle = Console.ReadLine()!;

                var searchedBooks = bookService.SearchByTitle(searchTitle);

                foreach(var b in searchedBooks)
                {
                    Console.WriteLine($"{b.Id} | {b.Title}");
                }

                break;

            case "11":
                Console.Write("Book Id: ");
                int bookId = int.Parse(Console.ReadLine()!);

                var copy = new BookCopy
                            {
                                BookId = bookId,
                                Status = BookCopyStatus.Available
                            };

                bookService.AddBookCopy(copy);
                Console.WriteLine("Book Copy Added");
                break;

            case "12":
                Console.Write("Book Copy Id: ");
                int damagedCopyId = int.Parse(Console.ReadLine()!);

                bookService.MarkBookCopyAsDamaged(damagedCopyId);
                Console.WriteLine("Book Copy Marked Damaged");
                break;

            case "13":
                Console.Write("Member Id: ");
                int borrowMemberId = int.Parse(Console.ReadLine()!);

                Console.Write("Book Copy Id: ");
                int borrowCopyId = int.Parse(Console.ReadLine()!);

                borrowingService.BorrowBook(
                    borrowMemberId,
                    borrowCopyId);
                Console.WriteLine("Book Borrowed");

                break;

            case "14":
                Console.Write("Borrowing Id: ");
                int borrowingId = int.Parse(Console.ReadLine()!);

                borrowingService.ReturnBook(borrowingId);
                Console.WriteLine("Book Returned");
                break;

            case "15":
                Console.Write("Member Id: ");
                int fineMemberId = int.Parse(Console.ReadLine()!);

                var fines = fineService.GetPendingFines(fineMemberId);

                foreach(var fine in fines)
                {
                    Console.WriteLine($"Fine Id: {fine.Id} | Amount: {fine.Amount}");
                }
                break;

            case "16":
                var activeBorrowings = reportService.GetActiveBorrowings();

                foreach(var borrowing in activeBorrowings)
                {
                    Console.WriteLine($"Borrowing Id: {borrowing.Id} | Member Id: {borrowing.MemberId}");
                }

                break;

            case "17":
                var b00ks = borrowingService.GetMostBorrowedBooks();

                foreach(var b00k in b00ks)
                {
                    Console.WriteLine($"Title: {b00k.Title} | Borrow Count: {b00k.Count}");
                }

                break;

            case "18":
                Console.Write("Member Id: ");
                int historyMemberId = int.Parse(Console.ReadLine()!);
                var history = reportService.GetBorrowingHistory(historyMemberId);

                foreach(var borrowing in history)
                {
                    Console.WriteLine($"Borrowing Id: {borrowing.Id} | Borrow Date: {borrowing.BorrowDate}");
                }
                break;

            case "19":
                running = false;
                break;

            default:
                Console.WriteLine("Invalid Choice");
                break;
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}