using MovieApp.Models.DTO;

namespace MovieApp.Respositories.Abstract
{
    // interfejs uwierzytelniania użytkownika
    public interface IUserAuthenticationService
    {
        // Task -  Reprezentuje operację asynchroniczną.
        // Logowanie
        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
        //Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username);
    }
}
