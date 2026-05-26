using Microsoft.EntityFrameworkCore;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;

namespace LibraryManagement.DataAccess.Repositories;

public class BookAuthorRepository : GenericRepository<BookAuthor>, IBookAuthorRepository
{
    public BookAuthorRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<BookAuthor?> GetByBookAndAuthorAsync(int bookId, int authorId)
    {
        return await _dbSet.FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);
    }

    public async Task<IEnumerable<BookAuthor>> GetByBookAsync(int bookId)
    {
        return await _dbSet
            .Where(ba => ba.BookId == bookId)
            .Include(ba => ba.Author)
            .Include(ba => ba.Book)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookAuthor>> GetByAuthorAsync(int authorId)
    {
        return await _dbSet
            .Where(ba => ba.AuthorId == authorId)
            .Include(ba => ba.Book)
            .Include(ba => ba.Author)
            .ToListAsync();
    }
}
