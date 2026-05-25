using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorController> _logger;

    public AuthorController(IAuthorService authorService, IMapper mapper, ILogger<AuthorController> logger)
    {
        _authorService = authorService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorResponseDTO>>> GetAll()
    {
        var authors = await _authorService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<AuthorResponseDTO>>(authors));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorResponseDTO>> GetById(int id)
    {
        var author = await _authorService.GetByIdAsync(id);
        if (author == null)
            return NotFound(new { message = $"No se encontró el autor con ID {id}" });

        return Ok(_mapper.Map<AuthorResponseDTO>(author));
    }

    [HttpPost]
    public async Task<ActionResult<AuthorResponseDTO>> Create([FromBody] AuthorRequestDTO request)
    {
        try
        {
            var author = _mapper.Map<Author>(request);
            var created = await _authorService.CreateAsync(author);
            var response = _mapper.Map<AuthorResponseDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AuthorRequestDTO request)
    {
        try
        {
            var author = _mapper.Map<Author>(request);
            await _authorService.UpdateAsync(id, author);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _authorService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
