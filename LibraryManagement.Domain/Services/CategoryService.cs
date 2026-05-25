using Microsoft.Extensions.Logging;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(
        ICategoryRepository categoryRepository,
        ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all categories");
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving category with ID: {CategoryId}", id);

        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            _logger.LogWarning("Category with ID {CategoryId} not found", id);

        return category;
    }

    public async Task<Category> CreateAsync(Category category)
    {
        var existingCategory = await _categoryRepository.GetByNameAsync(category.Name);
        if (existingCategory != null)
        {
            _logger.LogWarning("Category with name '{CategoryName}' already exists", category.Name);
            throw new InvalidOperationException(
                $"Ya existe una categoría con el nombre '{category.Name}'");
        }

        _logger.LogInformation("Creating category: {CategoryName}", category.Name);
        return await _categoryRepository.CreateAsync(category);
    }

    public async Task UpdateAsync(int id, Category category)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
        {
            _logger.LogWarning("Category with ID {CategoryId} not found for update", id);
            throw new KeyNotFoundException(
                $"No se encontró la categoría con ID {id}");
        }

        if (!string.Equals(existingCategory.Name, category.Name, StringComparison.OrdinalIgnoreCase))
        {
            var categoryWithSameName = await _categoryRepository.GetByNameAsync(category.Name);
            if (categoryWithSameName != null)
            {
                throw new InvalidOperationException(
                    $"Ya existe una categoría con el nombre '{category.Name}'");
            }
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;

        _logger.LogInformation("Updating category with ID: {CategoryId}", id);
        await _categoryRepository.UpdateAsync(existingCategory);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _categoryRepository.ExistsAsync(id);
        if (!exists)
        {
            _logger.LogWarning("Category with ID {CategoryId} not found for deletion", id);
            throw new KeyNotFoundException(
                $"No se encontró la categoría con ID {id}");
        }

        _logger.LogInformation("Deleting category with ID: {CategoryId}", id);
        await _categoryRepository.DeleteAsync(id);
    }
}