using PokemonReview.Models;

namespace PokemonReview.Interface
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();

        public Reviewer GetReviewer(int reviewerId);

        ICollection<Review> GetReviewerOfReview(int reviewerId);

        bool ReviewerExists(int reviewerId);
    }
}
