using Microsoft.Extensions.Logging;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class BookAuthorService : IBookAuthorService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookAuthorRepository _bookAuthorRepository;
    private readonly ILogger<BookAuthorService> _logger;

    public BookAuthorService(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IBookAuthorRepository bookAuthorRepository,
        ILogger<BookAuthorService> logger)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _bookAuthorRepository = bookAuthorRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<BookAuthor>> GetAuthorsByBookAsync(int bookId)
    {
        if (!await _bookRepository.ExistsAsync(bookId))
            throw new KeyNotFoundException($"No se encontró el libro con ID {bookId}");

        return await _bookAuthorRepository.GetByBookAsync(bookId);
    }

    public async Task<BookAuthor> LinkAuthorToBookAsync(int bookId, int authorId)
    {
        if (!await _bookRepository.ExistsAsync(bookId))
            throw new KeyNotFoundException($"No se encontró el libro con ID {bookId}");

        if (!await _authorRepository.ExistsAsync(authorId))
            throw new KeyNotFoundException($"No se encontró el autor con ID {authorId}");

        var existing = await _bookAuthorRepository.GetByBookAndAuthorAsync(bookId, authorId);
        if (existing != null)
            throw new InvalidOperationException("Ese autor ya está vinculado a ese libro");

        var entity = new BookAuthor
        {
            BookId = bookId,
            AuthorId = authorId
        };

        _logger.LogInformation("Linking author {AuthorId} to book {BookId}", authorId, bookId);
        return await _bookAuthorRepository.CreateAsync(entity);
    }

    public async Task UnlinkAuthorFromBookAsync(int bookId, int authorId)
    {
        var existing = await _bookAuthorRepository.GetByBookAndAuthorAsync(bookId, authorId);
        if (existing == null)
            throw new KeyNotFoundException("No existe la relación entre el libro y el autor");

        _logger.LogInformation("Unlinking author {AuthorId} from book {BookId}", authorId, bookId);
        await _bookAuthorRepository.DeleteAsync(existing.Id);
    }
}
