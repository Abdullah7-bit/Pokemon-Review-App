﻿using PokemonReview.Models;
using Microsoft.EntityFrameworkCore;

namespace PokemonReview.Interface
{
    public interface ICountryRepository
    {
        public ICollection<Country> GetCountries();

        Country GetCountry(int id);

        Country GetCountryByOwnerId(int ownerId);

        ICollection<Owner> GetOwnersFormACountry(int countryId);

        bool CountryExist(int id);

        bool CreateCountry(Country country);

        bool UpdateCountry(Country country);

        bool DeleteCountry(Country country);

        bool Save();
    }
}
