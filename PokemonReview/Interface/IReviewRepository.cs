﻿using PokemonReview.Models;

namespace PokemonReview.Interface
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();

        Review GetReview(int reviewId);

        ICollection<Review> GetReviewsOfAPokemon(int pokeId);

        bool ReviewExist(int reviewId);

        bool CreateReview(Review review);

        bool Save();
    }
}
