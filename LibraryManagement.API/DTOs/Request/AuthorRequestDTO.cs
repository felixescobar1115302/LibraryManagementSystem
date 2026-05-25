namespace LibraryManagement.API.DTOs.Request;

public class AuthorRequestDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Country { get; set; }
}
