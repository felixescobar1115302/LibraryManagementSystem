using Microsoft.EntityFrameworkCore;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;

namespace LibraryManagement.DataAccess.Repositories;

public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Author?> GetByFullNameAsync(string firstName, string lastName)
    {
        return await _dbSet.FirstOrDefaultAsync(a =>
            a.FirstName.ToLower() == firstName.ToLower() &&
            a.LastName.ToLower() == lastName.ToLower());
    }
}
