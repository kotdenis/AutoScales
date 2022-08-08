using AutoScales.Client.Services.Interfaces;
using AutoScales.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AutoScales.Client.Providers
{
    public class CustomStateProvider : AuthenticationStateProvider
    {
        private CurrentUser _currentUser;
        private readonly IAuthService _authService;
        public CustomStateProvider(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetCurrentUser();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, _currentUser.UserName) }.Concat(_currentUser.UserClaims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private async Task<CurrentUser> GetCurrentUser()
        {
            if (_currentUser != null && _currentUser.IsAuthenticated) return _currentUser;
            _currentUser = await _authService.CurrentUserInfoAsync();
            return _currentUser;
        }
        public async Task<bool> LogoutAsync()
        {
            var isOut = await _authService.LogoutAsync();
            _currentUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return isOut;
        }
        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            var isLogged = await _authService.LoginAsync(loginModel);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return isLogged;
        }
        public async Task<bool> RegisterAsync(RegisterModel registerModel)
        {
            var isRegistered = await _authService.RegisterAsync(registerModel);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return isRegistered;
        }
    }
}
