using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LMSDALLibrary.Context;

public class LibraryDbContextFactory
    : IDesignTimeDbContextFactory<LibraryDbContext>
{
    public LibraryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<LibraryDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=lmsdb;Username=murasaki;Password=123456");

        return new LibraryDbContext(optionsBuilder.Options);
    }
}