using AutoMapper;
using PokemonReview.Models;
using PokemonReview.Dto;

namespace PokemonReview.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            /*
             * While creating profile we can Add :
             * CreateMap<Pokemon, PokemonDto>().ReverseMap()
             * To avoid repetition of doing Vice Versa for all the DTOs.
             */

            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>();

        }
    }
}
