using LibraryManagement.Domain.Enums;

namespace LibraryManagement.API.DTOs.Request;

public class BookRequestDTO
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public BookFormat Format { get; set; }
    public int CategoryId { get; set; }
    public int AuthorId { get; set; }
}
