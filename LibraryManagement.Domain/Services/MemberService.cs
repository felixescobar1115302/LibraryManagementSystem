using Microsoft.Extensions.Logging;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<MemberService> _logger;

    public MemberService(IMemberRepository memberRepository, ILogger<MemberService> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _memberRepository.GetAllAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _memberRepository.GetByIdAsync(id);
    }

    public async Task<Member> CreateAsync(Member member)
    {
        var existing = await _memberRepository.GetByEmailAsync(member.Email);
        if (existing != null)
            throw new InvalidOperationException($"Ya existe un miembro con el email '{member.Email}'");

        return await _memberRepository.CreateAsync(member);
    }

    public async Task UpdateAsync(int id, Member member)
    {
        var existing = await _memberRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el miembro con ID {id}");

        if (!string.Equals(existing.Email, member.Email, StringComparison.OrdinalIgnoreCase))
        {
            var sameEmail = await _memberRepository.GetByEmailAsync(member.Email);
            if (sameEmail != null && sameEmail.Id != id)
                throw new InvalidOperationException($"Ya existe un miembro con el email '{member.Email}'");
        }

        existing.FirstName = member.FirstName;
        existing.LastName = member.LastName;
        existing.Email = member.Email;
        existing.Phone = member.Phone;
        existing.MembershipDate = member.MembershipDate;

        await _memberRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _memberRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró el miembro con ID {id}");

        await _memberRepository.DeleteAsync(id);
    }
}
