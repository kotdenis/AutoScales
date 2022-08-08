
using AutoScales.Core.Services.Interfaces;
using AutoScales.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoScales.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterModel model, CancellationToken ct = default)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            await _authService.CreateUserAsync(model, ct);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _authService.SignInAsync(loginModel, ct);
            return Ok();
        }

        [HttpGet("userinfo")]
        public async Task<ActionResult<CurrentUser>> GetCurrentUser(CancellationToken ct = default)
        {
            var result = await _authService.GetCurrentUserAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpPost("out")]
        public async Task<IActionResult> Logout(CancellationToken ct = default)
        {
            await _authService.LogoutAsync(ct); 
            return Ok();
        }
    }
}
