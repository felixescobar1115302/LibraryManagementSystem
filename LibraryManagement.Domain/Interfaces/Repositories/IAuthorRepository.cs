using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;

public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<Author?> GetByFullNameAsync(string firstName, string lastName);
}
