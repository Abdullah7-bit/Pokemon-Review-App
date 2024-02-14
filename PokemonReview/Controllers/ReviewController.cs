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
    public class ReviewController : ControllerBase
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        public ReviewController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            try
            {
                var reviews = _mapper.Map<List<Review>>(_reviewRepository.GetReviews());

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            try
            {
                if (!_reviewRepository.ReviewExist(reviewId))
                {
                    return NotFound();
                }
                var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetOwner by ID is ran into an Error. Details :  {ex}" });
            }

        }


        [HttpGet("pokemon/{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfAPokemon(int reviewId)
        {
            try
            {
                if (!_reviewRepository.ReviewExist(reviewId))
                {
                    return NotFound();
                }
                var pokemonreview = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(reviewId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(pokemonreview);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetPokemon Rating by ID is ran into an Error. Details :  {ex}" });

            }

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId,[FromBody] ReviewDto createReview)
        {
            try
            {
                if (createReview == null || string.IsNullOrWhiteSpace(createReview.Title))
                {
                    ModelState.AddModelError("", "Category Name cannot be empty!!");
                    return BadRequest(ModelState);
                }
                else
                {
                    var checking_review = _reviewRepository.GetReviews()
                        .Where(r => r.Title.Trim().ToUpper() == createReview.Title.TrimEnd().ToUpper())
                        .FirstOrDefault();
                    if (checking_review != null)
                    {
                        ModelState.AddModelError("", "Review already exists.");
                        return StatusCode(422, ModelState);
                    }
                    else if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        var reviewMap = _mapper.Map<Review>(createReview);

                        reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
                        reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

                        if (!_reviewRepository.CreateReview(reviewMap))
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


        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int reviewId, [FromBody] ReviewDto updateReview)
        {
            try
            {
                if (updateReview == null)
                {
                    return BadRequest(ModelState);
                }
                else if (reviewId != updateReview.Id)
                {
                    return BadRequest(ModelState);
                }
                else if (!_reviewRepository.ReviewExist(reviewId))
                {
                    return BadRequest(ModelState);
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var reviewMap = _mapper.Map<Review>(updateReview);
                    if (!_reviewRepository.UpdateReview(reviewMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while updating Review");
                        return StatusCode(500, ModelState);
                    }
                    return Ok("Review Updated Successfully!!");

                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error while executing the update/PUT API for the Review, Details: {ex}" });
            }
        }
    }
}
