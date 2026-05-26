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

        CreateMap<AuthorRequestDTO, Author>();
        CreateMap<Author, AuthorResponseDTO>();

        CreateMap<BookRequestDTO, Book>();
        CreateMap<Book, BookResponseDTO>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AuthorName,
                opt => opt.MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));

        CreateMap<MemberRequestDTO, Member>();
        CreateMap<Member, MemberResponseDTO>();

        CreateMap<LoanRequestDTO, Loan>();
        CreateMap<Loan, LoanResponseDTO>()
            .ForMember(dest => dest.MemberName,
                opt => opt.MapFrom(src => src.Member.FirstName + " " + src.Member.LastName))
            .ForMember(dest => dest.BookTitle,
                opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => (int)src.Status));
    }
}
