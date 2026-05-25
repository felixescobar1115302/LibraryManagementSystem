using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities;

public class Book : AuditBase
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public BookFormat Format { get; set; } = BookFormat.Physical;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
}
