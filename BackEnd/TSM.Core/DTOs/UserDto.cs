using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TSM.Core.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? TeamId { get; set; }
        public List<int> TechnologyIds { get; set; } = new List<int>();
        public string? TeamName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }

    public class UserCreateDto
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? TeamId { get; set; }
        public List<int> TechnologyIds { get; set; } = new List<int>();
        public bool IsAdmin { get; set; }
    }

    public class UserUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? TeamId { get; set; }
        public List<int> TechnologyIds { get; set; } = new List<int>();
        public bool IsAdmin { get; set; }
    }

    public class UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public UserDto User { get; set; } = new UserDto();
    }
} 