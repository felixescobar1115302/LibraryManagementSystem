using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/book/{bookId}/authors")]
public class BookAuthorController : ControllerBase
{
    private readonly IBookAuthorService _bookAuthorService;
    private readonly IMapper _mapper;

    public BookAuthorController(IBookAuthorService bookAuthorService, IMapper mapper)
    {
        _bookAuthorService = bookAuthorService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookAuthorResponseDTO>>> GetByBook(int bookId)
    {
        try
        {
            var items = await _bookAuthorService.GetAuthorsByBookAsync(bookId);
            return Ok(_mapper.Map<IEnumerable<BookAuthorResponseDTO>>(items));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<BookAuthorResponseDTO>> Create(int bookId, [FromBody] BookAuthorRequestDTO request)
    {
        try
        {
            var created = await _bookAuthorService.LinkAuthorToBookAsync(bookId, request.AuthorId);
            var response = _mapper.Map<BookAuthorResponseDTO>(created);
            return CreatedAtAction(nameof(GetByBook), new { bookId }, response);
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

    [HttpDelete("{authorId}")]
    public async Task<IActionResult> Delete(int bookId, int authorId)
    {
        try
        {
            await _bookAuthorService.UnlinkAuthorFromBookAsync(bookId, authorId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
