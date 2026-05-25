using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(
        ICategoryService categoryService,
        IMapper mapper,
        ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        var response = _mapper.Map<IEnumerable<CategoryResponseDTO>>(categories);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponseDTO>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
            return NotFound(new { message = $"No se encontró la categoría con ID {id}" });

        var response = _mapper.Map<CategoryResponseDTO>(category);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDTO>> Create([FromBody] CategoryRequestDTO request)
    {
        try
        {
            var category = _mapper.Map<Category>(request);
            var createdCategory = await _categoryService.CreateAsync(category);
            var response = _mapper.Map<CategoryResponseDTO>(createdCategory);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business validation error while creating category");
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating category");
            return StatusCode(500, new { message = "Ocurrió un error interno del servidor" });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryRequestDTO request)
    {
        try
        {
            var category = _mapper.Map<Category>(request);
            await _categoryService.UpdateAsync(id, category);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Category not found while updating");
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business validation error while updating category");
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating category");
            return StatusCode(500, new { message = "Ocurrió un error interno del servidor" });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Category not found while deleting");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting category");
            return StatusCode(500, new { message = "Ocurrió un error interno del servidor" });
        }
    }
}