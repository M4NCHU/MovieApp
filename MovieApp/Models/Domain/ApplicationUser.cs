using Microsoft.AspNetCore.Identity;

namespace MovieApp.Models.Domain
{
    // model Użytkownika
    public class ApplicationUser : IdentityUser
    {
        // Nazwa użytkownika
        public string Name { get; set; }
    }
}
