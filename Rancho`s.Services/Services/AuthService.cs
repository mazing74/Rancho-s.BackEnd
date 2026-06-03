using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rancho_s.core.Entities;
using Rancho_s.Services.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rancho_s.Services.Services
{
    public class AuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("This email is already registered.");
            var user = new AppUser
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.ToLower().Trim(),
                UserName = dto.Email.ToLower().Trim(),
                // Identity uses UserName for login — we use email as username
                PhoneNumber = dto.PhoneNumber.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ",
                      result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
            await _userManager.AddToRoleAsync(user, "Customer");
            
            var token = await GenerateJwtTokenAsync(user);

            return await BuildAuthResponseAsync(user,token);
        }


        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !user.IsActive)
                throw new Exception("Invalid email or password.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
                throw new Exception("Invalid email or password.");

            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var token = await GenerateJwtTokenAsync(user);
            return await BuildAuthResponseAsync(user, token);
        }

        public async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expiry = DateTime.UtcNow.AddDays(
               int.Parse(_configuration["JWT:ExpiryDays"] ?? "7"));

            var secret = _configuration["JWT:Secret"]
               ?? throw new Exception("JWT Secret not configured.");

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));


            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private async Task<AuthResponseDto> BuildAuthResponseAsync(AppUser user, string token)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthResponseDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? "Customer",
                Token = token,
                TokenExpiry = DateTime.UtcNow.AddDays(7)

            };
        }



    }
}
