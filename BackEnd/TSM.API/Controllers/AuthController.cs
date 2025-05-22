using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TSM.Core.DTOs;
using TSM.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Update last login time
                user.LastLogin = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                if (user.IsAdmin)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }

                var token = GetToken(authClaims);

                return Ok(new UserLoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Username = user.UserName ?? string.Empty,
                        Email = user.Email ?? string.Empty,
                        FirstName = user.FirstName ?? string.Empty,
                        LastName = user.LastName ?? string.Empty,
                        TeamId = user.TeamId,
                        IsAdmin = user.IsAdmin,
                        CreatedAt = user.CreatedAt,
                        LastLogin = user.LastLogin
                    }
                });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status409Conflict, new { Message = "User already exists!" });

            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
                return StatusCode(StatusCodes.Status409Conflict, new { Message = "Email already in use!" });

            ApplicationUser user = new()
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                TeamId = model.TeamId,
                IsAdmin = model.IsAdmin,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { Message = "User creation failed!", Errors = result.Errors.Select(e => e.Description) });

            return Ok(new { Message = "User created successfully!" });
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { Message = "User not authenticated" });
            }

            var user = await _userManager.FindByNameAsync(username);
            
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Failed to change password", Errors = result.Errors.Select(e => e.Description) });
            }

            return Ok(new { Message = "Password changed successfully" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var secret = _configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT:Secret is not configured");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            
            var expiryMinutesStr = _configuration["JWT:ExpiryMinutes"] ?? "60";
            var expiryMinutes = int.Parse(expiryMinutesStr);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
} 