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

    public BookController(
        IBookService bookService,
        IMapper mapper,
        ILogger<BookController> logger)
    {
        _bookService = bookService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        var response = _mapper.Map<IEnumerable<BookResponseDTO>>(books);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponseDTO>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book == null)
            return NotFound(new { message = $"No se encontró el libro con ID {id}" });

        var response = _mapper.Map<BookResponseDTO>(book);
        return Ok(response);
    }

    [HttpGet("category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetByCategory(int categoryId)
    {
        try
        {
            var books = await _bookService.GetByCategoryAsync(categoryId);
            var response = _mapper.Map<IEnumerable<BookResponseDTO>>(books);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Category not found while filtering books by category");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while filtering books by category");
            return StatusCode(500, new { message = "Ocurrió un error interno del servidor" });
        }
    }

    [HttpGet("author/{authorId:int}")]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetByAuthor(int authorId)
    {
        try
        {
            var books = await _bookService.GetByAuthorAsync(authorId);
            var response = _mapper.Map<IEnumerable<BookResponseDTO>>(books);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Author not found while filtering books by author");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while filtering books by author");
            return StatusCode(500, new { message = "Ocurrió un error interno del servidor" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDTO>> Create([FromBody] BookRequestDTO request)
    {
        try
        {
            var book = _mapper.Map<Book>(request);

            book.BookAuthors = request.AuthorIds
                .Select(authorId => new BookAuthor
                {
                    AuthorId = authorId
                })
                .ToList();

            var createdBook = await _bookService.CreateAsync(book);
            var response = _mapper.Map<BookResponseDTO>(createdBook);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business validation error while creating book");
            return Conflict(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Referenced entity not found while creating book");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating book");
            return StatusCode(500, new { message = "Ocurrió un error interno del servidor" });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookRequestDTO request)
    {
        try
        {
            var book = _mapper.Map<Book>(request);

            book.BookAuthors = request.AuthorIds
                .Select(authorId => new BookAuthor
                {
                    AuthorId = authorId
                })
                .ToList();

            await _bookService.UpdateAsync(id, book);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business validation error while updating book");
            return Conflict(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Referenced entity not found while updating book");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating book");
            return StatusCode(500, new { message = "Ocurrió un error interno del servidor" });
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
            _logger.LogWarning(ex, "Book not found while deleting");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting book");
            return StatusCode(500, new { message = "Ocurrió un error interno del servidor" });
        }
    }
}