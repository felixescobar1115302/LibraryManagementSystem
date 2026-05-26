using Microsoft.Extensions.Logging;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<LoanService> _logger;

    public LoanService(
        ILoanRepository loanRepository,
        IMemberRepository memberRepository,
        IBookRepository bookRepository,
        ILogger<LoanService> logger)
    {
        _loanRepository = loanRepository;
        _memberRepository = memberRepository;
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _loanRepository.GetAllWithDetailsAsync();
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await _loanRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task<Loan> CreateAsync(Loan loan)
    {
        var member = await _memberRepository.GetByIdAsync(loan.MemberId);
        if (member == null)
            throw new KeyNotFoundException($"No se encontró el miembro con ID {loan.MemberId}");

        var book = await _bookRepository.GetByIdAsync(loan.BookId);
        if (book == null)
            throw new KeyNotFoundException($"No se encontró el libro con ID {loan.BookId}");

        if (loan.DueDate < loan.LoanDate)
            throw new InvalidOperationException("DueDate no puede ser menor que LoanDate");

        var activeLoan = await _loanRepository.GetActiveLoanByBookIdAsync(loan.BookId);
        if (activeLoan != null)
            throw new InvalidOperationException("El libro ya tiene un préstamo activo");

        loan.Status = LoanStatus.Active;

        return await _loanRepository.CreateAsync(loan);
    }

    public async Task MarkAsReturnedAsync(int id, DateTime returnDate)
    {
        var loan = await _loanRepository.GetByIdAsync(id);
        if (loan == null)
            throw new KeyNotFoundException($"No se encontró el préstamo con ID {id}");

        if (returnDate < loan.LoanDate)
            throw new InvalidOperationException("ReturnDate no puede ser menor que LoanDate");

        loan.ReturnDate = returnDate;
        loan.Status = returnDate > loan.DueDate ? LoanStatus.Late : LoanStatus.Returned;

        await _loanRepository.UpdateAsync(loan);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _loanRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró el préstamo con ID {id}");

        await _loanRepository.DeleteAsync(id);
    }
}
