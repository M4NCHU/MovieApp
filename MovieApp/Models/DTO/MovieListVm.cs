using MovieApp.Models.Domain;

namespace MovieApp.Models.DTO
{
    // model listy filmów
    public class MovieListVm
    {
        // IQueryable - dane zostaną przefiltrowane po stronie bazy danych i do aplikacji zwrócone zostanie top 5 wyników
        public IQueryable<Movie> MovieList { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Term { get; set; }
    }
}
