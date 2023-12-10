using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Models.Domain
{
    // Model filmów
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? ReleaseYear { get; set; }

        public string? MovieImage { get; set; }  // stores movie image name with extension (eg, image0001.jpg)
        [Required]
        public string? Desc { get; set; }
        [Required]
        public string? Cast { get; set; }
        [Required]
        public string? Director { get; set; }


        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        //NotMapped określa, że każda właściwość, która jest obsługiwanym typem danych, jest reprezentowana w bazie danych.
        [NotMapped]
        [Required]
        public List<int>? Genres { get; set; }
        [NotMapped]
        // IEnumerable - wymóg pętli foreach
        public IEnumerable<SelectListItem>? GenreList { get; set; }
        [NotMapped]
        public string? GenreNames { get; set; }

        [NotMapped]
        // Wybór kilku gatunków filmowych
        public MultiSelectList? MultiGenreList { get; set; }
    }
}
