using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;

namespace cks_kaas.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null) return Unauthorized();
            return Ok(new { data = token });
        }
    }
}
