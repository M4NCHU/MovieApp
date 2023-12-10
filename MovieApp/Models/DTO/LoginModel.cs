using System.ComponentModel.DataAnnotations;

namespace MovieApp.Models.DTO
{
    // model logowania
    public class LoginModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
