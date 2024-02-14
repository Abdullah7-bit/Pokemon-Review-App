using AutoMapper;
using PokemonReview.Data;
using PokemonReview.Interface;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CountryRepository(DataContext datacontext, IMapper mapper)
        {
            _dataContext = datacontext;
            _mapper = mapper;
        }
        public bool CountryExist(int id)
        {
            return _dataContext.Countries.Any(c => c.Id == id);

        }

        public bool CreateCountry(Country country)
        {
            _dataContext.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _dataContext.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _dataContext.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _dataContext.Countries.Where(ce => ce.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwnerId(int ownerId)
        {
            return _dataContext.Owners.Where(o => o.Id == ownerId).Select(c=>c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFormACountry(int countryId)
        {
            return _dataContext.Owners.Where(o => o.Country.Id == countryId).ToList();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _dataContext.Update(country);
            return Save();
        }
    }
}
