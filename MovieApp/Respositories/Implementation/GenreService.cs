using MovieApp.Respositories.Abstract;
using MovieApp.Models.Domain;

namespace MovieApp.Respositories.Implementation
{
    // Implementacja interfejsu GenreService
    public class GenreService : IGenreService
    {
        // Pobierz kontekst bazy danych
        private readonly DatabaseContext ctx;
        public GenreService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }

        // Dodaj gatunek
        public bool Add(Genre model)
        {
            try
            {
                // Dodaj gatunek do tabeli Genre w bazie danych
                ctx.Genre.Add(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Usuń gatunek
        public bool Delete(int id)
        {
            try
            {
                // Wywołaj metodę pobierającą id gatunku filmowego
                var data = this.GetById(id);
                // Zwróć fałsz, jeżeli nie ma gatunku filmowego o tym id
                if (data == null)
                    return false;
                // Usuń gatunek z tabeli Genre w bazie danych
                ctx.Genre.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Pobierz gatunek po jego id
        public Genre GetById(int id)
        {
            // Znajdz gatunek o id z argumentu w bazie danych
            return ctx.Genre.Find(id);
        }

        // IQueryable - dane zostaną przefiltrowane po stronie bazy danych i do aplikacji zwrócone zostanie top 5 wyników
        public IQueryable<Genre> List()
        {
            // AsQueryable - Konwertuje element IEnumerable na .IQueryable
            var data = ctx.Genre.AsQueryable();
            return data;
        }

        // Edytuj gatunek
        public bool Update(Genre model)
        {
            try
            {
                // Edytuj gatunek w bazie danych
                ctx.Genre.Update(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
