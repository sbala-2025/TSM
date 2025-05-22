using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TSM.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? TeamId { get; set; }
        public int? TechnologyId { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        // Navigation properties
        public Team? Team { get; set; }
        public Technology? Technology { get; set; }
        public ICollection<UserTechnology> UserTechnologies { get; set; } = new List<UserTechnology>();
    }
} 