using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities;

public class Loan : AuditBase
{
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Active;

    public Member Member { get; set; } = null!;
    public Book Book { get; set; } = null!;
}
