using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;

public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<IEnumerable<Loan>> GetAllWithDetailsAsync();
    Task<Loan?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Loan>> GetByMemberAsync(int memberId);
    Task<IEnumerable<Loan>> GetByBookAsync(int bookId);
    Task<IEnumerable<Loan>> GetActiveLoansAsync();
    Task<Loan?> GetActiveLoanByBookIdAsync(int bookId);
}