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

        public bool CreateOwner(Owner owner)
        {
            _datacontext.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _datacontext.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _datacontext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerofAPokemon(int pokeId)
        {
            return _datacontext.PokemonOwners.Where(po => po.Pokemon.Id == pokeId).Select(p => p.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _datacontext.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
           return _datacontext.PokemonOwners.Where(po => po.Owner.Id == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool OwnerExist(int ownerId)
        {
            return _datacontext.Owners.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {
            var saved = _datacontext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _datacontext.Update(owner);
            return Save();
        }
    }
}