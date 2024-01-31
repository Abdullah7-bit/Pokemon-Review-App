using PokemonReview.Models;
using Microsoft.EntityFrameworkCore;

namespace PokemonReview.Interface
{
    public interface ICountryRepository
    {
        public ICollection<Country> GetCountries();

        Country GetCountry(int id);

        Country GetCountryById(int ownerId);

        ICollection<Owner> GetOwnersFormACountry(int ownerId);

        bool CountryExist(int id);
    }
}
