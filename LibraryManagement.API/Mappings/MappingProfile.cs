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

        // Book: el mapeo desde request a entity se hace parcial
        CreateMap<BookRequestDTO, Book>()
            .ForMember(dest => dest.BookAuthors, opt => opt.Ignore());

        CreateMap<Book, BookResponseDTO>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AuthorIds,
                opt => opt.MapFrom(src => src.BookAuthors.Select(ba => ba.AuthorId).ToList()))
            .ForMember(dest => dest.AuthorNames,
                opt => opt.MapFrom(src => src.BookAuthors
                    .Select(ba => ba.Author.FirstName + " " + ba.Author.LastName)
                    .ToList()));

        CreateMap<MemberRequestDTO, Member>();
        CreateMap<Member, MemberResponseDTO>();

        CreateMap<LoanRequestDTO, Loan>();
        CreateMap<Loan, LoanResponseDTO>()
            .ForMember(dest => dest.MemberName,
                opt => opt.MapFrom(src => src.Member.FirstName + " " + src.Member.LastName))
            .ForMember(dest => dest.BookTitle,
                opt => opt.MapFrom(src => src.Book.Title));

        CreateMap<BookAuthorRequestDTO, BookAuthor>();
        CreateMap<BookAuthor, BookAuthorResponseDTO>()
            .ForMember(dest => dest.BookTitle,
                opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.AuthorName,
                opt => opt.MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));
    }
}