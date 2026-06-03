using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rancho_s.Services.DTOs;
using Rancho_s.Services.Services;

namespace Rancho_s_Wilson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        // ─────────────────────────────────────────
        // POST /api/auth/register
        // Public — anyone can create a customer account
        // ─────────────────────────────────────────
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(
            [FromBody] RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        // ─────────────────────────────────────────
        // POST /api/auth/login
        // Public — get your token here
        // ─────────────────────────────────────────

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(
            [FromBody] LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }


    }
}
