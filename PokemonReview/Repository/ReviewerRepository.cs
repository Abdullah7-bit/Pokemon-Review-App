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

        public bool CreateReviewer(Reviewer reviewer)
        {
            _dataContext.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _dataContext.Remove(reviewer);
            return Save();
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

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _dataContext.Update(reviewer);
            return Save();
        }

        ICollection<Review> IReviewerRepository.GetReviewerOfReview(int reviewerId)
        {
            return _dataContext.Reviews.Where(rr => rr.Reviewer.Id == reviewerId).ToList();
        }
    }
}
