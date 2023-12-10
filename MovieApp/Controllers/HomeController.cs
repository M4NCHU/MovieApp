using Microsoft.AspNetCore.Mvc;
using MovieApp.Respositories.Abstract;

namespace MovieApp.Controllers
{

    // Kontroler Strony głównej
    public class HomeController : Controller
    {
        // Pobierz interfejs IMovieService tylko do odczytu
        private readonly IMovieService _movieService;

        // Konstruktor
        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // Metoda Index Wyświetlająca stronę główną.
        // Pobiera ona 2 argumenty, term czyli nazwę filmu oraz aktualną podstonę
        public IActionResult Index(string term = "", int currentPage = 1)
        {
            var movies = _movieService.List(term, true, currentPage);
            return View(movies);
        }

        // Metoda About wyświetla stronę z informacjami o stronie
        public IActionResult About()
        {
            return View();
        }

        // Metoda MovieDetail wyświetla szczegółowe informajce o filmie
        // Pobiera argument id potrzebny do wyświetlenia z bazy danych filmu o konkretnym id
        public IActionResult MovieDetail(int movieId)
        {
            var movie = _movieService.GetById(movieId);
            return View(movie);
        }

    }
}
