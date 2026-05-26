using Microsoft.EntityFrameworkCore;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces.Repositories;

namespace LibraryManagement.DataAccess.Repositories;

public class LoanRepository : GenericRepository<Loan>, ILoanRepository
{
    public LoanRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Loan>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(l => l.Member)
            .Include(l => l.Book)
            .ThenInclude(b => b.Author)
            .ToListAsync();
    }

    public async Task<Loan?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(l => l.Member)
            .Include(l => l.Book)
            .ThenInclude(b => b.Author)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Loan>> GetByMemberAsync(int memberId)
    {
        return await _dbSet
            .Where(l => l.MemberId == memberId)
            .Include(l => l.Member)
            .Include(l => l.Book)
            .ThenInclude(b => b.Author)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetByBookAsync(int bookId)
    {
        return await _dbSet
            .Where(l => l.BookId == bookId)
            .Include(l => l.Member)
            .Include(l => l.Book)
            .ThenInclude(b => b.Author)
            .ToListAsync();
    }

    public async Task<Loan?> GetActiveLoanByBookIdAsync(int bookId)
    {
        return await _dbSet
            .Include(l => l.Member)
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => l.BookId == bookId && l.Status == LoanStatus.Active);
    }
}
