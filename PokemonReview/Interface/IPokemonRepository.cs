using PokemonReview.Models;

namespace PokemonReview.Interface
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();


    }
}
