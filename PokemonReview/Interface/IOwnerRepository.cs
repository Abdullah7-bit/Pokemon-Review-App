using PokemonReview.Models;

namespace PokemonReview.Interface
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();

        Owner GetOwner(int ownerId);

        ICollection<Owner> GetOwnerofAPokemon(int pokeId);

        ICollection<Pokemon> GetPokemonByOwner(int ownerId);

        bool OwnerExist(int ownerId);
    }
}
