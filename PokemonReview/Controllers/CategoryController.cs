using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Models;
using PokemonReview.Interface;
using Microsoft.AspNetCore.Http.HttpResults;

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


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto createCategory)
        {
            try
            {
                if (createCategory == null || string.IsNullOrWhiteSpace(createCategory.Name))
                {
                    ModelState.AddModelError("", "Category Name cannot be empty!!");
                    return BadRequest(ModelState);
                }
                else
                {
                    var checking_category = _categoryrepository.GetCategories()
                        .Where(cc => cc.Name.Trim().ToUpper() == createCategory.Name.TrimEnd().ToUpper())
                        .FirstOrDefault();
                    if (checking_category != null)
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
                        var categoryMap = _mapper.Map<Category>(createCategory);
                        if (!_categoryrepository.CreateCategory(categoryMap))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving!!");
                            return StatusCode(500, ModelState);
                        }
                    }
                    return Ok("Successfully Created!!");
                    
                }

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error While executing Create/POST API for Category, Details:  {ex}" });
            }           
            
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updateCategory)
        {
            try
            {
                if (updateCategory == null)
                {
                    return BadRequest(ModelState);
                }
                else if(categoryId != updateCategory.Id)
                {
                    return BadRequest(ModelState);
                }
                else if(!_categoryrepository.CategoryExists(categoryId))
                {
                    return BadRequest(ModelState);
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var categoryMap = _mapper.Map<Category>(updateCategory);
                    if (!_categoryrepository.UpdateCategory(categoryMap)){
                        ModelState.AddModelError("","Something went wrong while updating Category");
                        return StatusCode(500,ModelState);
                    }
                    
                   return Ok("Category Updated Successfully!!");
                   
                }

            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error while executing the update/PUT API for the Category, Details: {ex}" });
            }
        }

    }
}
