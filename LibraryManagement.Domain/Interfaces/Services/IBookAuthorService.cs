using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Services;

public interface IBookAuthorService
{
    Task<IEnumerable<BookAuthor>> GetAuthorsByBookAsync(int bookId);
    Task<BookAuthor> LinkAuthorToBookAsync(int bookId, int authorId);
    Task UnlinkAuthorFromBookAsync(int bookId, int authorId);
}
