using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;


namespace MovieApp.Models.Domain
{
    // model gatunków filmowych, tabela zawierająca id filmu oraz id gatunku, co pozwala na póżniejsze ich połącznie
    public class MovieGenre
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int GenreId { get; set; }
    }
}
