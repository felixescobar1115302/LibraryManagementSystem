namespace LibraryManagement.Domain.Entities;

public class Author : AuditBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Country { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
