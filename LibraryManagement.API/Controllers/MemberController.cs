using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly IMapper _mapper;

    public MemberController(IMemberService memberService, IMapper mapper)
    {
        _memberService = memberService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponseDTO>>> GetAll()
    {
        var members = await _memberService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<MemberResponseDTO>>(members));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberResponseDTO>> GetById(int id)
    {
        var member = await _memberService.GetByIdAsync(id);
        if (member == null)
            return NotFound(new { message = $"No se encontró el miembro con ID {id}" });

        return Ok(_mapper.Map<MemberResponseDTO>(member));
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponseDTO>> Create([FromBody] MemberRequestDTO request)
    {
        try
        {
            var member = _mapper.Map<Member>(request);
            var created = await _memberService.CreateAsync(member);
            var response = _mapper.Map<MemberResponseDTO>(created);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MemberRequestDTO request)
    {
        try
        {
            var member = _mapper.Map<Member>(request);
            await _memberService.UpdateAsync(id, member);
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
            await _memberService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
