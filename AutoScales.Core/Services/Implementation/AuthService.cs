using AutoScales.Core.Services.Interfaces;
using AutoScales.Data.Entities;
using AutoScales.Data.Repositories.Interfaces;
using AutoScales.Shared.Constants;
using AutoScales.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AutoScales.Core.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IAuthRepository authRepository,
            IHttpContextAccessor contextAccessor)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _authRepository = authRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> CreateUserAsync(RegisterModel registerModel, CancellationToken ct)
        {
            var userByEmail = await _userManager.FindByEmailAsync(registerModel.Email);
            var userByName = await _userManager.FindByNameAsync(registerModel.UserName);
            if (userByEmail != null && userByName != null)
                throw new NullReferenceException($"This email or name are already in use.");
            else
            {
                var user = new User
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                };
                using var transaction = await _authRepository.BeginTransactionAsync(ct);
                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync(RoleConstants.User);
                    if (role == null)
                        await _roleManager.CreateAsync(new Role { Name = RoleConstants.User });
                    await _userManager.AddToRoleAsync(user, RoleConstants.User);
                    await _authRepository.CommitAsync(transaction, ct);
                    return true;
                }
                else
                {
                    await _authRepository.RollBackAsync(transaction, ct);
                    return false;
                }
            }
        }

        public async Task<bool> SignInAsync(LoginModel loginModel, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
                throw new NullReferenceException($"No such user with this email {nameof(loginModel.Email)}");
            if (await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>();
                if (roles.Any(x => x == RoleConstants.Admin))
                {
                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, RoleConstants.Admin),
                    };
                }
                else
                {
                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, RoleConstants.User),
                    };
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await _contextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return true;
            }
            else return false;
        }

        public Task<CurrentUser> GetCurrentUserAsync()
        {
            var user = _contextAccessor.HttpContext?.User;
            var currentUser = new CurrentUser()
            {
                IsAuthenticated = user.Identity.IsAuthenticated,
                UserName = user.Identity.Name,
                UserClaims = user.Claims.ToDictionary(c => c.Type, c => c.Value)
            };
            return Task.FromResult(currentUser);
        }

        public async Task LogoutAsync(CancellationToken ct)
        {
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _signInManager.SignOutAsync();
        }
    }
}
