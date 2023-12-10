using Microsoft.AspNetCore.Identity;
using MovieApp.Models.Domain;
using MovieApp.Models.DTO;
using MovieApp.Respositories.Abstract;
using System.Security.Claims;

namespace MovieApp.Respositories.Implementation
{
    // Implementacja interfejsu uwierzytelniania użytkownika
    public class UserAuthenticationService : IUserAuthenticationService
    {
        // Ustawia daną userName wartość dla określonego user elementu .
        private readonly UserManager<ApplicationUser> userManager;
        // Udostępnia interfejsy API do zarządzania rolami w magazynie trwałości.
        private readonly RoleManager<IdentityRole> roleManager;
        // Służy do określania, czy użytkownik jest uznawany za zablokowany.
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

        }

        // Rejestracja użytkownika
        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            // Nowa instancja klasy Status
            var status = new Status();

            // Sprawdzanie czy użytkownika o podanej nazwie istnieje
            var userExists = await userManager.FindByNameAsync(model.Username);

            // Jeżeli tak
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "Użytkownik o podanej nazwie istnieje";
                return status;
            }

            // Nowa instancja klasy ApplicationUser
            ApplicationUser user = new ApplicationUser()
            {
                // Przypisanie podanych danych do zmiennych
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Name = model.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            // Utwórz użytkownika o podanych danych
            var result = await userManager.CreateAsync(user, model.Password);
            // Jeżeli błąd
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Wystąpił błąd podczas tworzenia użytkownika";
                return status;
            }

            // Dodawanie ról dla użytkownika jeżeli rola nie istnieje
            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            // Dodawanie ról dla użytkownika jeżeli rola istnieje
            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "Zarejestrowano pomyślnie";
            return status;
        }


        // Logowanie
        // Task - Reprezentuje operację asynchroniczną.
        public async Task<Status> LoginAsync(LoginModel model)
        {
            // Nowa instancja klasy Status
            var status = new Status();
            // Sprawdzanie czy użytkownika o podanej nazwie istnieje
            var user = await userManager.FindByNameAsync(model.Username);
            // Jeżeli nie
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Niepoprawna nazwa użytkownika";
                return status;
            }
            // Sprawdzanie hasła
            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Niepoprawne hasło";
                return status;
            }

            // Logowanie
            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
            // Jeżeli pomyślne
            if (signInResult.Succeeded)
            {
                // Pobierz role
                var userRoles = await userManager.GetRolesAsync(user);
                // Autoryzacja oparta na oświadczeniach
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                // Wybierz wszystkie role przypisane użytkowniki
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logowanie pomyślne";
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "Użytkownik zablokowany";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Błąd podczas logowania";
            }

            return status;
        }

        // Wylogowanie
        public async Task LogoutAsync()
        {
            // Metoda służąca do wylogowania
            await signInManager.SignOutAsync();

        }
    }
}
