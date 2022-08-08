using AutoScales.Shared.Models;

namespace AutoScales.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> CreateUserAsync(RegisterModel registerModel, CancellationToken ct);
        Task<bool> SignInAsync(LoginModel loginModel, CancellationToken ct);
        Task<CurrentUser> GetCurrentUserAsync();
        Task LogoutAsync(CancellationToken ct);
    }
}
