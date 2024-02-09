using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interface;
using PokemonReview.Models;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
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


        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int reviewId)
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
    }
}
