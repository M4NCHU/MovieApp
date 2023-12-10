using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Models.Domain;
using MovieApp.Respositories.Abstract;


// Kontroler odpowiedzialny za gatunki filmowe

namespace MovieApp.Controllers
{
    [Authorize]
    public class GenreController : Controller
    {
        // odczyt interfejsu gatunków filmowych
        private readonly IGenreService _genreService;

        // Konstruktor
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // Dodawnie widoku
        public IActionResult Add()
        {
            return View();
        }


        // Dodawanie gatunków filmowych
        [HttpPost]
        public IActionResult Add(Genre model)
        {
            // Walidacja modelu
            if (!ModelState.IsValid)
                return View(model);

            // Przejdz do interfejsu z gatunkami i wybierz metodę Add
            var result = _genreService.Add(model);

            // Jeżeli dodano pomyślnie, wyświetl komunikat i przekieruj na stronę dodawania gatunków filmowych
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

        // Edytowanie gatunków filmowych
        public IActionResult Edit(int id)
        {
            var data = _genreService.GetById(id);
            return View(data);
        }

        // Modyfikowanie gatunków filmowych
        [HttpPost]
        public IActionResult Update(Genre model)
        {
            // Walidacja modelu
            if (!ModelState.IsValid)
                return View(model);
            // Przejdz do interfejsu z gatunkami i wybierz metodę Update
            var result = _genreService.Update(model);

            // Jeżeli Edytowano pomyślnie, wyświetl komunikat i przekieruj na stronę listy gatunków filmowych
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(GenreList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        // Pobieranie listy gatunków filmowych
        public IActionResult GenreList()
        {
            // Przejdz do interfejsu z gatunkami i wybierz metodę List
            var data = this._genreService.List().ToList();
            return View(data);
        }

        // Usuwanie gatunków filmowych
        public IActionResult Delete(int id)
        {
            var result = _genreService.Delete(id);
            return RedirectToAction(nameof(GenreList));
        }



    }
}
