using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TSM.Core.DTOs;
using TSM.Core.Models;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userManager.Users
                .Include(u => u.Team)
                .Include(u => u.UserTechnologies)
                .ToListAsync();
            
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                TeamId = u.TeamId,
                TechnologyIds = u.UserTechnologies.Select(ut => ut.TechnologyId).ToList(),
                TeamName = u.Team?.Name,
                IsAdmin = u.IsAdmin,
                CreatedAt = u.CreatedAt,
                LastLogin = u.LastLogin
            });

            return Ok(userDtos);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Team)
                .Include(u => u.UserTechnologies)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Check if the user has permission to view this user
            // Admin users can view any user, but non-admin users can only view their own profile
            if (!User.IsInRole("Admin") && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id)
            {
                return Forbid();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TeamId = user.TeamId,
                TechnologyIds = user.UserTechnologies.Select(ut => ut.TechnologyId).ToList(),
                TeamName = user.Team?.Name,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };

            return userDto;
        }

        // GET: api/Users/Current
        [HttpGet("Current")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.Users
                .Include(u => u.Team)
                .Include(u => u.UserTechnologies)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TeamId = user.TeamId,
                TechnologyIds = user.UserTechnologies.Select(ut => ut.TechnologyId).ToList(),
                TeamName = user.Team?.Name,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };

            return userDto;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if the user has permission to update this user
            // Admin users can update any user, but non-admin users can only update their own profile
            if (!User.IsInRole("Admin") && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id)
            {
                return Forbid();
            }

            // Non-admin users cannot grant admin rights
            if (!User.IsInRole("Admin") && userDto.IsAdmin)
            {
                return BadRequest("You do not have permission to grant admin rights.");
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.TeamId = userDto.TeamId;
            // Update UserTechnology records
            if (userDto.TechnologyIds != null)
            {
                user.UserTechnologies.Clear();
                foreach (var tid in userDto.TechnologyIds)
                {
                    user.UserTechnologies.Add(new UserTechnology { UserId = user.Id, TechnologyId = tid });
                }
            }
            
            // Only admin users can change admin status
            if (User.IsInRole("Admin"))
            {
                user.IsAdmin = userDto.IsAdmin;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { Message = "User update failed!", Errors = result.Errors.Select(e => e.Description) });
            }

            return NoContent();
        }

        // POST: api/Users/ChangePassword
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { Message = "Password change failed!", Errors = result.Errors.Select(e => e.Description) });
            }

            return Ok(new { Message = "Password changed successfully!" });
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { Message = "User deletion failed!", Errors = result.Errors.Select(e => e.Description) });
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] TSM.Core.DTOs.UserCreateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userManager.FindByNameAsync(userDto.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status409Conflict, new { Message = "User already exists!" });

            var emailExists = await _userManager.FindByEmailAsync(userDto.Email);
            if (emailExists != null)
                return StatusCode(StatusCodes.Status409Conflict, new { Message = "Email already in use!" });

            var user = new ApplicationUser
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                TeamId = userDto.TeamId,
                IsAdmin = userDto.IsAdmin,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { Message = "User creation failed!", Errors = result.Errors.Select(e => e.Description) });

            // Add UserTechnology records
            if (userDto.TechnologyIds != null && userDto.TechnologyIds.Count > 0)
            {
                user.UserTechnologies = userDto.TechnologyIds.Select(tid => new UserTechnology { UserId = user.Id, TechnologyId = tid }).ToList();
                await _userManager.UpdateAsync(user);
            }

            return Ok(new { Message = "User created successfully!" });
        }
    }
} 