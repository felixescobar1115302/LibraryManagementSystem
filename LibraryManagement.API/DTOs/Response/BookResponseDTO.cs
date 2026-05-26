using LibraryManagement.Domain.Enums;

namespace LibraryManagement.API.DTOs.Response;

public class BookResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int Stock { get; set; }
    public BookFormat Format { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public List<int> AuthorIds { get; set; } = new();
    public List<string> AuthorNames { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}