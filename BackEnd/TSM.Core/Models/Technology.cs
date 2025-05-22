using System;
using System.Collections.Generic;

namespace TSM.Core.Models
{
    public class Technology
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Category? Category { get; set; }
        public ICollection<TechnologyStatus> TechnologyStatuses { get; set; } = new List<TechnologyStatus>();
        public ICollection<ProjectTechnology> ProjectTechnologies { get; set; } = new List<ProjectTechnology>();
        public ICollection<TechnologySkill> TechnologySkills { get; set; } = new List<TechnologySkill>();
        public ICollection<UserTechnology> UserTechnologies { get; set; } = new List<UserTechnology>();
    }
} 