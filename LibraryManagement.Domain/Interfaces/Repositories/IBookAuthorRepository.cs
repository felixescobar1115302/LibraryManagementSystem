using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;

public interface IBookAuthorRepository : IGenericRepository<BookAuthor>
{
    Task<BookAuthor?> GetByBookAndAuthorAsync(int bookId, int authorId);
    Task<IEnumerable<BookAuthor>> GetByBookAsync(int bookId);
    Task<IEnumerable<BookAuthor>> GetByAuthorAsync(int authorId);
}
