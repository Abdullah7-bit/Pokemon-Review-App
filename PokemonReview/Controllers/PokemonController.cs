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
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonrepository;
        //private readonly IOwnerRepository _ownerRepository;
        //private readonly IPokemonRepository _pokemonrepository;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonRepository pokemonrepository, IMapper mapper)
        {
            _pokemonrepository = pokemonrepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemon()
        {
            try
            {
                var pokemon = _mapper.Map<List<PokemonDto>>(_pokemonrepository.GetPokemons());

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(pokemon);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            try
            {
                if (!_pokemonrepository.PokemonExists(pokeId))
                {
                    return NotFound();
                }
                var pokemon = _mapper.Map<PokemonDto>(_pokemonrepository.GetPokemon(pokeId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(pokemon);
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetPokemon by ID is ran into an Error. Details :  {ex}" });
            }
            
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId) 
        {
            try
            {
                if (!_pokemonrepository.PokemonExists(pokeId))
                {
                    return NotFound();
                }
                var pokemonrating = _pokemonrepository.GetPokemonRating(pokeId);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                return Ok(pokemonrating);

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetPokemon Rating by ID is ran into an Error. Details :  {ex}" });
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto createPokemon)
        {
            try
            {
                if (createPokemon == null || string.IsNullOrWhiteSpace(createPokemon.Name))
                {
                    ModelState.AddModelError("", "Category Name cannot be empty!!");
                    return BadRequest(ModelState);
                }
                else
                {
                    var checking_pokemon = _pokemonrepository.GetPokemons()
                        .Where(cc => cc.Name.Trim().ToUpper() == createPokemon.Name.TrimEnd().ToUpper())
                        .FirstOrDefault();
                    if (checking_pokemon != null)
                    {
                        ModelState.AddModelError("", "Pokemon already exists.");
                        return StatusCode(422, ModelState);
                    }
                    else if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        var pokemonMap = _mapper.Map<Pokemon>(createPokemon);
                        if (!_pokemonrepository.CreatePokemon(ownerId, categoryId, pokemonMap))
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error While executing Create/POST API for Category, Details:  {ex}" });
            }
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry([FromQuery] int ownerId, int pokeId, [FromQuery] int categoryId, [FromBody] PokemonDto updatePokemon)
        {
            try
            {
                if (updatePokemon == null)
                {
                    return BadRequest(ModelState);
                }
                else if (pokeId != updatePokemon.Id)
                {
                    return BadRequest(ModelState);
                }
                else if (!_pokemonrepository.PokemonExists(pokeId))
                {
                    return BadRequest(ModelState);
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var pokemonMap = _mapper.Map<Pokemon>(updatePokemon);


                    if (!_pokemonrepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while updating Pokemon");
                        return StatusCode(500, ModelState);
                    }
                    return Ok("Pokemon Updated Successfully!!");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error while executing the update/PUT API for the Pokemon, Details: {ex}" });
            }
        }
    }
}
