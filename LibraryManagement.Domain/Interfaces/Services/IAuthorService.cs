using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Services;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);
    Task<Author> CreateAsync(Author author);
    Task UpdateAsync(int id, Author author);
    Task DeleteAsync(int id);
}
