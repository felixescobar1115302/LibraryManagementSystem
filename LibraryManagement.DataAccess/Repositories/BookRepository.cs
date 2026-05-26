using Microsoft.EntityFrameworkCore;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;

namespace LibraryManagement.DataAccess.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _dbSet
            .Include(b => b.Category)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .ToListAsync();
    }

    public override async Task<Book?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Category)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        return await _dbSet
            .Include(b => b.Category)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.ISBN.ToLower() == isbn.ToLower());
    }

    public async Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Where(b => b.CategoryId == categoryId)
            .Include(b => b.Category)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByAuthorAsync(int authorId)
    {
        return await _dbSet
            .Where(b => b.BookAuthors.Any(ba => ba.AuthorId == authorId))
            .Include(b => b.Category)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .ToListAsync();
    }
}