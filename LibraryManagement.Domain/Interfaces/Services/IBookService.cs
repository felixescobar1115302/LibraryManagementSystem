using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task UpdateAsync(int id, Book book);
    Task DeleteAsync(int id);
    Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Book>> GetByAuthorAsync(int authorId);
}
