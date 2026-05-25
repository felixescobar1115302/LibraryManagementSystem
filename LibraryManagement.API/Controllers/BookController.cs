using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;
    private readonly ILogger<BookController> _logger;

    public BookController(IBookService bookService, IMapper mapper, ILogger<BookController> logger)
    {
        _bookService = bookService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<BookResponseDTO>>(books));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponseDTO>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null)
            return NotFound(new { message = $"No se encontró el libro con ID {id}" });

        return Ok(_mapper.Map<BookResponseDTO>(book));
    }

    [HttpGet("category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetByCategory(int categoryId)
    {
        try
        {
            var books = await _bookService.GetByCategoryAsync(categoryId);
            return Ok(_mapper.Map<IEnumerable<BookResponseDTO>>(books));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("author/{authorId:int}")]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetByAuthor(int authorId)
    {
        try
        {
            var books = await _bookService.GetByAuthorAsync(authorId);
            return Ok(_mapper.Map<IEnumerable<BookResponseDTO>>(books));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDTO>> Create([FromBody] BookRequestDTO request)
    {
        try
        {
            var book = _mapper.Map<Book>(request);
            var created = await _bookService.CreateAsync(book);
            var response = _mapper.Map<BookResponseDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookRequestDTO request)
    {
        try
        {
            var book = _mapper.Map<Book>(request);
            await _bookService.UpdateAsync(id, book);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
