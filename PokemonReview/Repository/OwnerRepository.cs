using PokemonReview.Data;
using PokemonReview.Interface;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _datacontext;
        public OwnerRepository(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        public Owner GetOwner(int ownerId)
        {
            return _datacontext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerofAPokemon(int pokeId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Owner> GetOwners()
        {
            return _datacontext.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            throw new NotImplementedException();
        }

        public bool OwnerExist(int ownerId)
        {
            return _datacontext.Owners.Any(o => o.Id == ownerId);
        }
    }
}
