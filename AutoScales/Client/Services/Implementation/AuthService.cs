using AutoScales.Client.Constants;
using AutoScales.Client.Services.Interfaces;
using AutoScales.Shared.Models;
using System.Net.Http.Json;

namespace AutoScales.Client.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrentUser> CurrentUserInfoAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<CurrentUser>(new Uri(ClientConstants.AuthUrl + "userinfo"));
            if(result == null)
                return new CurrentUser();
            return result;
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(ClientConstants.AuthUrl + "login"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(loginModel)
            };
            var result = await _httpClient.SendAsync(request);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest || result.StatusCode == System.Net.HttpStatusCode.InternalServerError) 
                return false;

            return true;
        }
        public async Task<bool> LogoutAsync()
        {
            var result = await _httpClient.PostAsync(new Uri(ClientConstants.AuthUrl + "out"), null);
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }
        public async Task<bool> RegisterAsync(RegisterModel registerModel)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(ClientConstants.AuthUrl + "signup"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(registerModel)
            };
            var result = await _httpClient.SendAsync(request);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return false;
            return true;
        }
    }
}
