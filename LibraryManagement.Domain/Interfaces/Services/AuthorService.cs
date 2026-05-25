using Microsoft.Extensions.Logging;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(IAuthorRepository authorRepository, ILogger<AuthorService> logger)
    {
        _authorRepository = authorRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Author>> GetAllAsync() =>
        await _authorRepository.GetAllAsync();

    public async Task<Author?> GetByIdAsync(int id) =>
        await _authorRepository.GetByIdAsync(id);

    public async Task<Author> CreateAsync(Author author)
    {
        var existing = await _authorRepository.GetByFullNameAsync(author.FirstName, author.LastName);
        if (existing != null)
            throw new InvalidOperationException(
                $"Ya existe un autor con el nombre '{author.FirstName} {author.LastName}'");

        return await _authorRepository.CreateAsync(author);
    }

    public async Task UpdateAsync(int id, Author author)
    {
        var existingAuthor = await _authorRepository.GetByIdAsync(id);
        if (existingAuthor == null)
            throw new KeyNotFoundException($"No se encontró el autor con ID {id}");

        if (!string.Equals(existingAuthor.FirstName, author.FirstName, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(existingAuthor.LastName, author.LastName, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _authorRepository.GetByFullNameAsync(author.FirstName, author.LastName);
            if (duplicate != null && duplicate.Id != id)
                throw new InvalidOperationException(
                    $"Ya existe un autor con el nombre '{author.FirstName} {author.LastName}'");
        }

        existingAuthor.FirstName = author.FirstName;
        existingAuthor.LastName = author.LastName;
        existingAuthor.Country = author.Country;

        await _authorRepository.UpdateAsync(existingAuthor);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _authorRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró el autor con ID {id}");

        await _authorRepository.DeleteAsync(id);
    }
}
