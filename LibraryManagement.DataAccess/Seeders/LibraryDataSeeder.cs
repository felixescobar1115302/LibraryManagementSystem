using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Seeders;

public static class LibraryDataSeeder
{
    public static async Task SeedAsync(LibraryDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() { Name = "Fiction", Description = "Novelas y narrativa" },
                new() { Name = "Science", Description = "Ciencia y divulgación" },
                new() { Name = "History", Description = "Historia universal" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        if (!await context.Authors.AnyAsync())
        {
            var authors = new List<Author>
            {
                new() { FirstName = "Gabriel", LastName = "García Márquez", Country = "Colombia" },
                new() { FirstName = "Julio", LastName = "Verne", Country = "Francia" },
                new() { FirstName = "Stephen", LastName = "Hawking", Country = "Reino Unido" }
            };

            await context.Authors.AddRangeAsync(authors);
            await context.SaveChangesAsync();
        }

        if (!await context.Books.AnyAsync())
        {
            var fictionId = await context.Categories.Where(c => c.Name == "Fiction").Select(c => c.Id).FirstAsync();
            var scienceId = await context.Categories.Where(c => c.Name == "Science").Select(c => c.Id).FirstAsync();
            var historyId = await context.Categories.Where(c => c.Name == "History").Select(c => c.Id).FirstAsync();

            var gaboId = await context.Authors.Where(a => a.LastName == "García Márquez").Select(a => a.Id).FirstAsync();
            var verneId = await context.Authors.Where(a => a.LastName == "Verne").Select(a => a.Id).FirstAsync();
            var hawkingId = await context.Authors.Where(a => a.LastName == "Hawking").Select(a => a.Id).FirstAsync();

            var books = new List<Book>
            {
                new() { Title = "Cien años de soledad", ISBN = "9780307474728", PublicationYear = 1967, Stock = 5, Format = BookFormat.Physical, CategoryId = fictionId },
                new() { Title = "Viaje al centro de la Tierra", ISBN = "9788420674179", PublicationYear = 1864, Stock = 4, Format = BookFormat.Physical, CategoryId = fictionId },
                new() { Title = "Breve historia del tiempo", ISBN = "9780553380163", PublicationYear = 1988, Stock = 6, Format = BookFormat.Ebook, CategoryId = scienceId },
                new() { Title = "Historia del siglo XX", ISBN = "9788484326749", PublicationYear = 1994, Stock = 3, Format = BookFormat.Physical, CategoryId = historyId }
            };

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();

            var savedBooks = await context.Books.ToListAsync();

            var relations = new List<BookAuthor>
            {
                new() { BookId = savedBooks.Single(b => b.Title == "Cien años de soledad").Id, AuthorId = gaboId },
                new() { BookId = savedBooks.Single(b => b.Title == "Viaje al centro de la Tierra").Id, AuthorId = verneId },
                new() { BookId = savedBooks.Single(b => b.Title == "Breve historia del tiempo").Id, AuthorId = hawkingId },
                new() { BookId = savedBooks.Single(b => b.Title == "Historia del siglo XX").Id, AuthorId = hawkingId }
            };

            await context.BookAuthors.AddRangeAsync(relations);
            await context.SaveChangesAsync();
        }

        if (!await context.Members.AnyAsync())
        {
            var members = new List<Member>
            {
                new() { FirstName = "Felix", LastName = "Escobar", Email = "felix@example.com", Phone = "3000000000", MembershipDate = DateTime.UtcNow.Date },
                new() { FirstName = "Maria", LastName = "Lopez", Email = "maria@example.com", Phone = "3010000000", MembershipDate = DateTime.UtcNow.Date }
            };

            await context.Members.AddRangeAsync(members);
            await context.SaveChangesAsync();
        }

        if (!await context.Loans.AnyAsync())
        {
            var memberId = await context.Members.Select(m => m.Id).FirstAsync();
            var bookId = await context.Books.Select(b => b.Id).FirstAsync();

            var loans = new List<Loan>
            {
                new()
                {
                    MemberId = memberId,
                    BookId = bookId,
                    LoanDate = DateTime.UtcNow.Date,
                    DueDate = DateTime.UtcNow.Date.AddDays(7),
                    Status = LoanStatus.Active
                }
            };

            await context.Loans.AddRangeAsync(loans);
            await context.SaveChangesAsync();
        }
    }
}
