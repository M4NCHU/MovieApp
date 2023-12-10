using MovieApp.Models.Domain;
using MovieApp.Models.DTO;

namespace MovieApp.Respositories.Abstract
{
    // Insterfejs Gatunków filmowych
    public interface IGenreService
    {
        // Dodaj gatunek
        bool Add(Genre model);
        // Edytuj gatunek
        bool Update(Genre model);
        // Pobierz gatunek po jego id
        Genre GetById(int id);

        // Usuń gatunek
        bool Delete(int id);

        // IQueryable - dane zostaną przefiltrowane po stronie bazy danych i do aplikacji zwrócone zostanie top 5 wyników
        // Lista gatunków filmowych
        IQueryable<Genre> List();

    }
}
