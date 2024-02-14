using AutoMapper;
using PokemonReview.Data;
using PokemonReview.Interface;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        // Implementing DatabaseContext and Mapper

        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public ReviewRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public bool CreateReview(Review review)
        {
            _dataContext.AddAsync(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _dataContext.Remove(review);
            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return _dataContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _dataContext.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _dataContext.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
        }

        public bool ReviewExist(int reviewId)
        {
            return _dataContext.Reviews.Any(r => r.Id == reviewId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _dataContext.Update(review);
            return Save();
        }
    }
}
