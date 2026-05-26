using Microsoft.Extensions.Logging;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookAuthorRepository _bookAuthorRepository;
    private readonly ILogger<BookService> _logger;

    public BookService(
        IBookRepository bookRepository,
        ICategoryRepository categoryRepository,
        IAuthorRepository authorRepository,
        IBookAuthorRepository bookAuthorRepository,
        ILogger<BookService> logger)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _authorRepository = authorRepository;
        _bookAuthorRepository = bookAuthorRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Book>> GetAllAsync() =>
        await _bookRepository.GetAllAsync();

    public async Task<Book?> GetByIdAsync(int id) =>
        await _bookRepository.GetByIdAsync(id);

    public async Task<Book> CreateAsync(Book book)
    {
        var existingBook = await _bookRepository.GetByIsbnAsync(book.ISBN);
        if (existingBook != null)
            throw new InvalidOperationException(
                $"Ya existe un libro con el ISBN '{book.ISBN}'");

        if (!await _categoryRepository.ExistsAsync(book.CategoryId))
            throw new KeyNotFoundException(
                $"No se encontró la categoría con ID {book.CategoryId}");

        if (book.BookAuthors == null || !book.BookAuthors.Any())
            throw new InvalidOperationException(
                "El libro debe tener al menos un autor asociado");

        foreach (var bookAuthor in book.BookAuthors)
        {
            if (!await _authorRepository.ExistsAsync(bookAuthor.AuthorId))
                throw new KeyNotFoundException(
                    $"No se encontró el autor con ID {bookAuthor.AuthorId}");
        }

        var createdBook = await _bookRepository.CreateAsync(book);

        foreach (var bookAuthor in book.BookAuthors)
        {
            bookAuthor.BookId = createdBook.Id;
            await _bookAuthorRepository.CreateAsync(bookAuthor);
        }

        return await _bookRepository.GetByIdAsync(createdBook.Id) ?? createdBook;
    }

    public async Task UpdateAsync(int id, Book book)
    {
        var existingBook = await _bookRepository.GetByIdAsync(id);
        if (existingBook == null)
            throw new KeyNotFoundException(
                $"No se encontró el libro con ID {id}");

        if (!string.Equals(existingBook.ISBN, book.ISBN, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _bookRepository.GetByIsbnAsync(book.ISBN);
            if (duplicate != null && duplicate.Id != id)
                throw new InvalidOperationException(
                    $"Ya existe un libro con el ISBN '{book.ISBN}'");
        }

        if (!await _categoryRepository.ExistsAsync(book.CategoryId))
            throw new KeyNotFoundException(
                $"No se encontró la categoría con ID {book.CategoryId}");

        if (book.BookAuthors == null || !book.BookAuthors.Any())
            throw new InvalidOperationException(
                "El libro debe tener al menos un autor asociado");

        foreach (var bookAuthor in book.BookAuthors)
        {
            if (!await _authorRepository.ExistsAsync(bookAuthor.AuthorId))
                throw new KeyNotFoundException(
                    $"No se encontró el autor con ID {bookAuthor.AuthorId}");
        }

        existingBook.Title = book.Title;
        existingBook.ISBN = book.ISBN;
        existingBook.PublicationYear = book.PublicationYear;
        existingBook.Stock = book.Stock;
        existingBook.Format = book.Format;
        existingBook.CategoryId = book.CategoryId;

        await _bookRepository.UpdateAsync(existingBook);

        var currentRelations = await _bookAuthorRepository.GetByBookAsync(id);
        foreach (var relation in currentRelations)
        {
            await _bookAuthorRepository.DeleteAsync(relation.Id);
        }

        foreach (var bookAuthor in book.BookAuthors)
        {
            bookAuthor.BookId = id;
            await _bookAuthorRepository.CreateAsync(bookAuthor);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _bookRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException(
                $"No se encontró el libro con ID {id}");

        var currentRelations = await _bookAuthorRepository.GetByBookAsync(id);
        foreach (var relation in currentRelations)
        {
            await _bookAuthorRepository.DeleteAsync(relation.Id);
        }

        await _bookRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId)
    {
        if (!await _categoryRepository.ExistsAsync(categoryId))
            throw new KeyNotFoundException(
                $"No se encontró la categoría con ID {categoryId}");

        return await _bookRepository.GetByCategoryAsync(categoryId);
    }

    public async Task<IEnumerable<Book>> GetByAuthorAsync(int authorId)
    {
        if (!await _authorRepository.ExistsAsync(authorId))
            throw new KeyNotFoundException(
                $"No se encontró el autor con ID {authorId}");

        return await _bookRepository.GetByAuthorAsync(authorId);
    }
}