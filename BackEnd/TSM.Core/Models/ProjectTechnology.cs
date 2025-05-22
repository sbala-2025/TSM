using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSM.Core.Models
{
    [Table("ProjectTechnologies")]
    public class ProjectTechnology
    {
        public int ProjectId { get; set; }
        public int TechnologyId { get; set; }
        public string? UsageDetails { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Project? Project { get; set; }
        public Technology? Technology { get; set; }
    }
} 