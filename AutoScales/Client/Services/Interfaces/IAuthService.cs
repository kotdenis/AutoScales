using AutoScales.Shared.Models;

namespace AutoScales.Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<CurrentUser> CurrentUserInfoAsync();
        Task<bool> LoginAsync(LoginModel loginModel);
        Task<bool> LogoutAsync();
        Task<bool> RegisterAsync(RegisterModel registerModel);
    }
}
