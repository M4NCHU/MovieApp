using MovieApp.Respositories.Abstract;
using MovieApp.Models.Domain;
using MovieApp.Models.DTO;

namespace MovieApp.Respositories.Implementation
{
    // Implementacja interfejsu MovieService
    public class MovieService : IMovieService
    {
        // Pobierz kontekst bazy danych
        private readonly DatabaseContext ctx;
        public MovieService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }

        // Dodaj film
        public bool Add(Movie model)
        {
            try
            {
                // Dodaj film do bazy danych
                ctx.Movie.Add(model);
                ctx.SaveChanges();
                // Dodaj wszystkie wybrane gatunki filmowe
                foreach (int genreId in model.Genres)
                {
                    // Utwórz instancję klasy MovieGenre
                    var movieGenre = new MovieGenre
                    {
                        MovieId = model.Id,
                        GenreId = genreId
                    };
                    // Dodaj wybrane gatunki filmowe do do tabeli w bazie danych
                    ctx.MovieGenre.Add(movieGenre);
                }
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Usuń film
        public bool Delete(int id)
        {
            try
            {
                // Pobierz gatunek filmowy po konkretym id
                var data = this.GetById(id);
                // Zwróć fałsz jeżeli nie ma takiego gatunku muzycznego
                if (data == null)
                    return false;
                // Znajdz w bazie danych gatunek o podanym id
                var movieGenres = ctx.MovieGenre.Where(a => a.MovieId == data.Id);
                foreach (var movieGenre in movieGenres)
                {
                    // Usuń gatunek muzyczny z bazy danych
                    ctx.MovieGenre.Remove(movieGenre);
                }
                // Usuń film z bazy danych
                ctx.Movie.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Pobierz film po jego id
        public Movie GetById(int id)
        {
            // Znajduje jednostkę z podanymi wartościami klucza podstawowego
            return ctx.Movie.Find(id);
        }

        // Pobierz listę filmów np. o określonej nazwie
        // Vm - View model
        public MovieListVm List(string term = "", bool paging = false, int currentPage = 0)
        {
            // nowa instancja klasy MovieListVm
            var data = new MovieListVm();

            // Pobierz listę filmów z bazy danych
            var list = ctx.Movie.ToList();


            // Jeżeli nie jest null albo pusty argument term wszyukaj go w bazie danych
            if (!string.IsNullOrEmpty(term))
            {
                // Zmień litery podane w argumencie na małe
                term = term.ToLower();
                // Wyszukaj w bazie filmy o podanej nazwie
                list = list.Where(a => a.Title.ToLower().StartsWith(term)).ToList();
            }

            // stronicowanie
            if (paging)
            {
                // Maksymalna ilość elementów na jednej stronie
                int pageSize = 5;
                // Zliczanie elementów w liście z bazy danych
                int count = list.Count;
                // Liczenie ilości stron jaka będzie po zliczeniu wszystkich elementów w liście
                int TotalPages = (int)Math.Ceiling(count / (double)pageSize);

                // Skip - Pomija określoną liczbę elementów w sekwencji, a następnie zwraca pozostałe elementy.
                list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                // Uzyskane zmienne dodaj do listy data
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = TotalPages;
            }

            // pobierz określoną ilość filmów z listy
            foreach (var movie in list)
            {
                // Pobranie gatunków
                var genres = (from genre in ctx.Genre
                              join mg in ctx.MovieGenre
                              on genre.Id equals mg.GenreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                // Łącznie otrzymanych gatunków
                var genreNames = string.Join(',', genres);
                movie.GenreNames = genreNames;
            }
            // AsQueryable - Konwertuje element IEnumerable na .IQueryable
            data.MovieList = list.AsQueryable();
            return data;
        }

        public bool Update(Movie model)
        {
            // Edycja filmów
            try
            {
                // Pobranie gatunków które posiada dany film i wybranie tych do usuniecia
                var genresToDeleted = ctx.MovieGenre.Where(a => a.MovieId == model.Id && !model.Genres.Contains(a.GenreId)).ToList();
                // Wybierz wszystkie wybrane gatunki i je usuń
                foreach (var mGenre in genresToDeleted)
                {
                    ctx.MovieGenre.Remove(mGenre);
                }
                // Dodawanie gatunków filmowych
                foreach (int genId in model.Genres)
                {
                    // FirstOrDefault - Zwraca pierwszy element sekwencji lub wartość domyślną, jeśli nie znaleziono żadnego elementu.
                    var movieGenre = ctx.MovieGenre.FirstOrDefault(a => a.MovieId == model.Id && a.GenreId == genId);
                    // Jeżeli nie ma gatunku
                    if (movieGenre == null)
                    {
                        // Nowa instancja klay MovieGenre
                        movieGenre = new MovieGenre { GenreId = genId, MovieId = model.Id };
                        // Dodaj gatunek do bazy danych
                        ctx.MovieGenre.Add(movieGenre);
                    }
                }
                // Edytuj film w bazie danych
                ctx.Movie.Update(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Pobierz listę gatunków przypisaną do danego filmu
        public List<int> GetGenreByMovieId(int movieId)
        {
            // Pobierz z bazy danych gatunki filmowe o podanym id
            var genreIds = ctx.MovieGenre.Where(a => a.MovieId == movieId).Select(a => a.GenreId).ToList();
            return genreIds;
        }

    }
}