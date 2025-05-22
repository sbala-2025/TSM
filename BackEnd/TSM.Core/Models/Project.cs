using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSM.Core.Models
{
    [Table("Projects")]
    public class Project
    {
        [Key]
        [Column("ProjectId")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public string Status { get; set; } = "Active";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<ProjectTechnology> ProjectTechnologies { get; set; } = new List<ProjectTechnology>();
        public ICollection<ProjectTeamMember> ProjectTeamMembers { get; set; } = new List<ProjectTeamMember>();
    }
} 