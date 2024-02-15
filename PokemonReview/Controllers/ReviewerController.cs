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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            try
            {
                var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(reviewers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviwer(int reviewerId)
        {
            try
            {
                if (!_reviewerRepository.ReviewerExists(reviewerId))
                {
                    return NotFound();
                }
                var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(reviewer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetReviewer by ID is ran into an Error. Details :  {ex}" });
            }

        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            try
            {
                if (!_reviewerRepository.ReviewerExists(reviewerId))
                {
                    return NotFound();
                }
                var reviewsbyReviewer = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewerOfReview(reviewerId));
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(reviewsbyReviewer);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"GetReviews by ReviewerID is ran into an Error. Details :  {ex}" });
            }


        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto createReviewer)
        {
            try
            {
                if (createReviewer == null || string.IsNullOrWhiteSpace(createReviewer.LastName))
                {
                    ModelState.AddModelError("", "Category Name cannot be empty!!");
                    return BadRequest(ModelState);
                }
                else
                {
                    var checking_reviewer = _reviewerRepository.GetReviewers()
                        .Where(cc => cc.LastName.Trim().ToUpper() == createReviewer.LastName.TrimEnd().ToUpper())
                        .FirstOrDefault();
                    if (checking_reviewer != null)
                    {
                        ModelState.AddModelError("", "Reviewer already exists.");
                        return StatusCode(422, ModelState);
                    }
                    else if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        var reviewerMap = _mapper.Map<Reviewer>(createReviewer);
                        if (!_reviewerRepository.CreateReviewer(reviewerMap))
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error While executing Create/POST API for Reviewer, Details:  {ex}" });
            }

        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updateReviewer)
        {
            try
            {
                if (updateReviewer == null)
                {
                    return BadRequest(ModelState);
                }
                else if (reviewerId != updateReviewer.Id)
                {
                    return BadRequest(ModelState);
                }
                else if (!_reviewerRepository.ReviewerExists(reviewerId))
                {
                    return BadRequest(ModelState);
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var reviewerMap = _mapper.Map<Reviewer>(updateReviewer);
                    if (!_reviewerRepository.UpdateReviewer(reviewerMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while updating Reviewer");
                        return StatusCode(500, ModelState);
                    }
                    return Ok("Reviewer Updated Successfully!!");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error while executing the update/PUT API for the Reviewer, Details: {ex}" });
            }
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound($"Reviewer {reviewerId} not found!!!");
            }
            else
            {
                var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong while Deleting Reviewer.");
                    return StatusCode(500, ModelState);
                }
                return Ok("Reviewer deleted successfully!!!");
            }
        }
    }
}
