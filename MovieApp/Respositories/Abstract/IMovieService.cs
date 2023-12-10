using MovieApp.Models.Domain;
using MovieApp.Models.DTO;

namespace MovieApp.Respositories.Abstract
{
    // interfejs filmów
    public interface IMovieService
    {
        // Dodaj film
        bool Add(Movie model);
        // Edytuj film
        bool Update(Movie model);
        // Pobierz film po jego id
        Movie GetById(int id);
        // Usuń film
        bool Delete(int id);
        // Pobierz listę filmów np. o określonej nazwie
        // Vm - View model
        MovieListVm List(string term = "", bool paging = false, int currentPage = 0);
        // Pobierz listę gatunków przypisaną do danego filmu
        List<int> GetGenreByMovieId(int movieId);

    }
}
