using Microsoft.AspNetCore.Mvc;

namespace PokemonReview.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
