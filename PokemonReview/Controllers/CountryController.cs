using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interface;
using PokemonReview.Models;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            try
            {
                var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            try
            {
                if (!_countryRepository.CountryExist(countryId))
                {
                    return NotFound();
                }
                var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(country);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetCountry by ID is ran into an Error. Details :  {ex}" });
            }

        }


        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Country))]

        public IActionResult GetCountryByOwnerId(int ownerId)
        {
            try
            {
                var country = _mapper.Map<CountryDto>(
                _countryRepository.GetCountryByOwnerId(ownerId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);

                }
                else
                {
                    return Ok(country);
                }

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetCountry by ownerID is ran into an Error. Details :  {ex}" });
            }
            
            
        }

    }
}
