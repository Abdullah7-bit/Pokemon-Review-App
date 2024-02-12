using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Interface;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public ReviewerRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public Reviewer GetReviewer(int reviewerId)
        {
            return _dataContext.Reviewers.Where(rr => rr.Id == reviewerId).Include(e=> e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _dataContext.Reviewers.ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _dataContext.Reviewers.Any(rr => rr.Id == reviewerId);
        }

        ICollection<Review> IReviewerRepository.GetReviewerOfReview(int reviewerId)
        {
            return _dataContext.Reviews.Where(rr => rr.Reviewer.Id == reviewerId).ToList();
        }
    }
}
