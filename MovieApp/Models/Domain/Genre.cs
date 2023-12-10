using Microsoft.Build.Framework;

namespace MovieApp.Models.Domain
{
    // Model gatunków filmowych
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string? GenreName { get; set; }
    }
}
