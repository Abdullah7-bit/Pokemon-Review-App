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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto createCountry)
        {
            try
            {
                if (createCountry == null || string.IsNullOrWhiteSpace(createCountry.Name))
                {
                    ModelState.AddModelError("", "Country Name cannot be empty!! ");
                    return BadRequest(ModelState);
                }
                else
                {
                    var checking_country = _countryRepository.GetCountries()
                        .Where(cc => cc.Name.Trim().ToUpper() == createCountry.Name.TrimEnd().ToUpper())
                        .FirstOrDefault();
                    if (checking_country != null)
                    {
                        ModelState.AddModelError("", "Category already exists.");
                        return StatusCode(422, ModelState);
                    }
                    else if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        var countryMap = _mapper.Map<Country>(createCountry);
                        if (!_countryRepository.CreateCountry(countryMap))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving!!");
                            return StatusCode(500, ModelState);
                        }
                    }
                    return Ok("Successfully Created!!");

                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error While executing Create/POST API for Country, Details:  {ex}" });
            }

        }


        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updateCountry)
        {
            try
            {
                if (updateCountry == null)
                {
                    return BadRequest(ModelState);
                }
                else if (countryId != updateCountry.Id)
                {
                    return BadRequest(ModelState);
                }
                else if (!_countryRepository.CountryExist(countryId))
                {
                    return BadRequest(ModelState);
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var countryMap = _mapper.Map<Country>(updateCountry);
                    if (!_countryRepository.UpdateCountry(countryMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while updating Country");
                        return StatusCode(500, ModelState);
                    }
                    return Ok("Country Updated Successfully!!");

                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error while executing the update/PUT API for the Country, Details: {ex}" });
            }
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
            {
                return NotFound($"Country {countryId} not found!!!");
            }
            else
            {
                var countryToDelete = _countryRepository.GetCountry(countryId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else if (!_countryRepository.DeleteCountry(countryToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong while Deleting Country.");
                    return StatusCode(500, ModelState);
                }
                return Ok("Country deleted successfully!!!");
            }
        }

    }
}
