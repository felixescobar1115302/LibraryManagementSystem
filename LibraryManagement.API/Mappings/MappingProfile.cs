using AutoMapper;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryRequestDTO, Category>();
        CreateMap<Category, CategoryResponseDTO>();
    }
}