using Microsoft.AspNetCore.Mvc;
using MovieApp.Models.DTO;
using MovieApp.Respositories.Abstract;

namespace MovieApp.Controllers
{
    // Kontroler uwierzytelniania użytkownika
    public class UserAuthenticationController : Controller
    {
        // Pobranie interfejsu uwieżytelniania
        private IUserAuthenticationService authService;

        // Konstruktor
        public UserAuthenticationController(IUserAuthenticationService authService)
        {
            this.authService = authService;
        }

        /*public async Task<IActionResult> Register()
        {
            // Tworzenie konta admina
            var model = new RegistrationModel
            {
                Email = "admin@gmail.com",
                Username = "admin",
                Name = "Maciej",
                Password = "Admin@123",
                PasswordConfirm = "Admin@123",
                Role = "Admin",
            };

            var result = await authService.RegisterAsync(model);
            
            return Ok(result.Message);
        }*/

        // Wyświetlnie Widoku logowania
        public async Task<IActionResult> Login()
        {
            return View();
        }


        // Logowanie użytkownika
        [HttpPost]

        public async Task<IActionResult> Login(LoginModel model)
        {
            // Walidacja modelu
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Wywołanie metody logowania użytkownika (asynchronicznie)
            var result = await authService.LoginAsync(model);

            // Jeżeli logowanie przebiegło pomyślnie przekieruj użytkownika na stronę główną
            if(result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = "Nie można zalogować";
                // Przekieruj użytkownika z powrotem na stronę logowania
                return RedirectToAction(nameof(Login));
            }
        }

        // Wyloguj użytkownika
        public async Task<IActionResult> Logout()
        {
            // Wywołanie funkcji wylogowania
            await authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

    }
}
