using LMSModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSDALLibrary.Context;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) {}

    public DbSet<Member> Members => Set<Member>();
    public DbSet<MembershipType> MembershipTypes {get; set;}
    public DbSet<Book> Books {get; set;}
    public DbSet<BookCategory> BookCategories {get; set;}
    public DbSet<BookCopy> BookCopies {get; set;}
    public DbSet<Borrowing> Borrowings {get; set;}
    public DbSet<Fine> Fines {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ScalarDecimalResult>().HasNoKey();
        
        // A member has one membership type
        modelBuilder.Entity<Member>().HasOne(m => m.MembershipType)
                                     .WithMany(mt => mt.Members)
                                     .HasForeignKey(m => m.MembershipTypeId)
                                     .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Member>()
                    .HasIndex(m => m.Phone)
                    .IsUnique();
        modelBuilder.Entity<Member>()
                    .HasIndex(m => m.Email)
                    .IsUnique();

        // A book belongs to one category
        modelBuilder.Entity<Book>().HasOne(b => b.Category)
                                   .WithMany(c => c.Books)
                                   .HasForeignKey(b => b.CategoryId)
                                   .OnDelete(DeleteBehavior.Restrict);
        
        // A book copy belongs to one book
        modelBuilder.Entity<BookCopy>().HasOne(bc => bc.Book)
                                       .WithMany(b => b.BookCopies)
                                       .HasForeignKey(bc => bc.BookId)
                                       .OnDelete(DeleteBehavior.Cascade);

        // A borrowing record belongs to one member
        modelBuilder.Entity<Borrowing>().HasOne(b => b.Member)
                                        .WithMany(m => m.Borrowings)
                                        .HasForeignKey(b => b.MemberId)
                                        .OnDelete(DeleteBehavior.Restrict);

        // A borrowing record belongs to one book copy
        modelBuilder.Entity<Borrowing>().HasOne(b => b.BookCopy)
                                        .WithMany(bc => bc.Borrowings)
                                        .HasForeignKey(b => b.BookCopyId)
                                        .OnDelete(DeleteBehavior.Restrict);

        // A borrowing can have one fine
        modelBuilder.Entity<Borrowing>().HasOne(b => b.Fine)
                                        .WithOne(f => f.Borrowing)
                                        .HasForeignKey<Fine>(f => f.BorrowingId)
                                        .OnDelete(DeleteBehavior.Cascade);

        // Store fine amount with 2 decimal places
        modelBuilder.Entity<Fine>().Property(f => f.Amount).HasColumnType("numeric(10,2)");
    }
}