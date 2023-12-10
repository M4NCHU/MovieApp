using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieApp.Models.Domain;
using MovieApp.Respositories.Abstract;

namespace MovieApp.Controllers
{
    // Authorize - odpowiada za pozwolenie dostępu do strony użytkownikowi o określonej roli
    [Authorize]
    public class MovieController : Controller
    {
        // Pobierz interfejsy z filmami, uploadem plików oraz gatunkami filmowymi
        private readonly IMovieService _movieService;
        private readonly IFileService _fileService;
        private readonly IGenreService _genService;

        // Konstruktor posiadający za atgumenty interfejsy
        public MovieController(IGenreService genService, IMovieService MovieService, IFileService fileService)
        {
            _movieService = MovieService;
            _fileService = fileService;
            _genService = genService;
        }

        // Dodaj Film
        public IActionResult Add()
        {
            // Pobierz instancje klasy Movie i przypisz ją do zmiennej model
            var model = new Movie();
            // pobierz listę gatunków muzycznych
            model.GenreList = _genService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Movie model)
        {
            // pobierz listę gatunków muzycznych
            model.GenreList = _genService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            // Walidacja modelu
            if (!ModelState.IsValid)
                return View(model);

            // Dodawanie zdjęcia do filmu, jeżeli zdjęcie zostało wybrane
            if (model.ImageFile != null)
            {
                // Zapisz zdjęcie
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                // Błąd przy próbie dodania zdjęcia
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "Nie można zapisać zdjęcia";
                    return View(model);
                }
                // Dodawanie zdjęcia
                var imageName = fileReult.Item2;
                model.MovieImage = imageName;
            }
            // Dodaj film
            var result = _movieService.Add(model);
            if (result)
            {
                TempData["msg"] = "Dodano pomyślnie";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Wystąpił błąd";
                return View(model);
            }
        }

        // Edytuj Film
        public IActionResult Edit(int id)
        {
            // Pobierz film po jego id
            var model = _movieService.GetById(id);
            // Pobierz przypisane gatunki filmowe do filmu
            var selectedGenres = _movieService.GetGenreByMovieId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Movie model)
        {
            // Pobierz film po jego id
            var selectedGenres = _movieService.GetGenreByMovieId(model.Id);
            // Pobierz przypisane gatunki filmowe do filmu
            MultiSelectList multiGenreList = new MultiSelectList(_genService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            // Walidacja modelu
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "Nie można zapisać zdjęcia";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.MovieImage = imageName;
            }
            var result = _movieService.Update(model);
            if (result)
            {
                TempData["msg"] = "Dodano pomyślnie";
                return RedirectToAction(nameof(MovieList));
            }
            else
            {
                TempData["msg"] = "Wystąpił błąd";
                return View(model);
            }
        }

        // Wyświetl listę filmów
        public IActionResult MovieList()
        {
            // Pobierz listę filmów
            var data = this._movieService.List();
            return View(data);
        }

        // Usuń film
        public IActionResult Delete(int id)
        {
            // Usuń film o określonym id
            var result = _movieService.Delete(id);
            return RedirectToAction(nameof(MovieList));
        }
    }
}
