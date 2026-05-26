using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities;

public class Book : AuditBase
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int Stock { get; set; }
    public BookFormat Format { get; set; }
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
