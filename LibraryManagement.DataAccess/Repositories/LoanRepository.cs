using Microsoft.EntityFrameworkCore;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;

namespace LibraryManagement.DataAccess.Repositories;

public class LoanRepository : GenericRepository<Loan>, ILoanRepository
{
    public LoanRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _dbSet
            .Include(l => l.Member)
            .Include(l => l.Book)
                .ThenInclude(b => b.Category)
            .Include(l => l.Book)
                .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetAllWithDetailsAsync()
    {
        return await GetAllAsync();
    }

    public override async Task<Loan?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(l => l.Member)
            .Include(l => l.Book)
                .ThenInclude(b => b.Category)
            .Include(l => l.Book)
                .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Loan?> GetByIdWithDetailsAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<IEnumerable<Loan>> GetByMemberAsync(int memberId)
    {
        return await _dbSet
            .Where(l => l.MemberId == memberId)
            .Include(l => l.Member)
            .Include(l => l.Book)
                .ThenInclude(b => b.Category)
            .Include(l => l.Book)
                .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetByBookAsync(int bookId)
    {
        return await _dbSet
            .Where(l => l.BookId == bookId)
            .Include(l => l.Member)
            .Include(l => l.Book)
                .ThenInclude(b => b.Category)
            .Include(l => l.Book)
                .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
    {
        return await _dbSet
            .Where(l => l.ReturnDate == null)
            .Include(l => l.Member)
            .Include(l => l.Book)
                .ThenInclude(b => b.Category)
            .Include(l => l.Book)
                .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
            .ToListAsync();
    }

    public async Task<Loan?> GetActiveLoanByBookIdAsync(int bookId)
    {
        return await _dbSet
            .Include(l => l.Member)
            .Include(l => l.Book)
                .ThenInclude(b => b.Category)
            .Include(l => l.Book)
                .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(l => l.BookId == bookId && l.ReturnDate == null);
    }
}