using PokemonReview.Models;

namespace PokemonReview.Interface
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);

        ICollection<Pokemon> GetPokemonbyCategory(int categoryId);

        bool CategoryExists(int id);

        bool CreateCategory(Category category);

        bool Save();

    }
}
