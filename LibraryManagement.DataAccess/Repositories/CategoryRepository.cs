using Microsoft.EntityFrameworkCore;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;

namespace LibraryManagement.DataAccess.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }
}