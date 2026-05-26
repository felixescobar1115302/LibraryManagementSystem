using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    private readonly ILoanService _loanService;
    private readonly IMapper _mapper;

    public LoanController(ILoanService loanService, IMapper mapper)
    {
        _loanService = loanService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoanResponseDTO>>> GetAll()
    {
        var loans = await _loanService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<LoanResponseDTO>>(loans));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanResponseDTO>> GetById(int id)
    {
        var loan = await _loanService.GetByIdAsync(id);
        if (loan == null)
            return NotFound(new { message = $"No se encontró el préstamo con ID {id}" });

        return Ok(_mapper.Map<LoanResponseDTO>(loan));
    }

    [HttpPost]
    public async Task<ActionResult<LoanResponseDTO>> Create([FromBody] LoanRequestDTO request)
    {
        try
        {
            var loan = _mapper.Map<Loan>(request);
            var created = await _loanService.CreateAsync(loan);
            var response = _mapper.Map<LoanResponseDTO>(created);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
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

    [HttpPatch("{id:int}/return")]
    public async Task<IActionResult> MarkAsReturned(int id, [FromQuery] DateTime returnDate)
    {
        try
        {
            await _loanService.MarkAsReturnedAsync(id, returnDate);
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
            await _loanService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
