using AutoMapper;
using PokemonReview.Models;
using PokemonReview.Dto;

namespace PokemonReview.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();

        }
    }
}
