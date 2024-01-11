using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interface;
using PokemonReview.Models;


namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonrepository;
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

    }
}
