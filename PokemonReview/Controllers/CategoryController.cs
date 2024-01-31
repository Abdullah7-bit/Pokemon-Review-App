using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Models;
using PokemonReview.Interface;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryrepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository,IMapper mapper)
        {
            _categoryrepository = categoryRepository;
            _mapper = mapper;

        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _mapper.Map<List<CategoryDto>>(_categoryrepository.GetCategories());
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {message= $"Error While Excuting Get Categories API, Details : {ex}"});
            }

        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type=typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            try
            {
                if (!_categoryrepository.CategoryExists(categoryId))
                {
                    return NotFound();
                }
                var category = _mapper.Map<CategoryDto>(_categoryrepository.GetCategory(categoryId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(category);

            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error While Excuting Get Categories by Id API, Details : {ex}" });
            }
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            try
            {
                var pokemonbycategory = _mapper.Map<List<PokemonDto>>(_categoryrepository.GetPokemonbyCategory(categoryId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(pokemonbycategory);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error while Executing Get Pokemon by Category Id API, Details : {ex}" });
            }
            
        }
    }
}
