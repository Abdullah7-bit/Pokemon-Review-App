using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interface;
using PokemonReview.Models;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository ,IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwner()
        {
            try
            {
                var owner = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            try
            {
                if (!_ownerRepository.OwnerExist(ownerId))
                {
                    return NotFound();
                }
                var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(owner);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetOwner by ID is ran into an Error. Details :  {ex}" });
            }

        }


        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            try
            {
                if (!_ownerRepository.OwnerExist(ownerId))
                {
                    return NotFound();
                }
                var pokemonowner = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(pokemonowner);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetPokemon Rating by ID is ran into an Error. Details :  {ex}" });

            }

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId,[FromBody] OwnerDto createOwner)
        {
            try
            {
                /*
                 * This API does not validating the following inputs with DB:
                 * firstName
                 * Gym
                 * 
                 * Q: How we are going to exclude the Foreign Key From Validating?
                 * 
                 */
                if (createOwner == null || string.IsNullOrWhiteSpace(createOwner.LastName))
                {
                    ModelState.AddModelError("", "Owner Name cannot be empty!!");
                    return BadRequest(ModelState);
                }
                else
                {
                    var checking_owner = _ownerRepository.GetOwners()
                        .Where(cc => cc.LastName.Trim().ToUpper() == createOwner.LastName.TrimEnd().ToUpper())
                        .FirstOrDefault();
                    if (checking_owner != null)
                    {
                        ModelState.AddModelError("", "Owner already exists.");
                        return StatusCode(422, ModelState);
                    }
                    else if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        var ownerMap = _mapper.Map<Owner>(createOwner);
                        ownerMap.Country = _countryRepository.GetCountry(countryId);
                        if (!_ownerRepository.CreateOwner(ownerMap))
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error While executing Create/POST API for Owner, Details:  {ex}" });
            }

        }

    }
}
