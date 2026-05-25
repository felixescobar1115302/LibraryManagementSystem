namespace LibraryManagement.Domain.Entities;

public class Category : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}