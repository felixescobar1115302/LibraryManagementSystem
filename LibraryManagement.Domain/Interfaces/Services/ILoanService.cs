using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Services;

public interface ILoanService
{
    Task<IEnumerable<Loan>> GetAllAsync();
    Task<Loan?> GetByIdAsync(int id);
    Task<Loan> CreateAsync(Loan loan);
    Task MarkAsReturnedAsync(int id, DateTime returnDate);
    Task DeleteAsync(int id);
}
