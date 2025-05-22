using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSM.Core.Models
{
    [Table("ProjectTeamMembers")]
    public class ProjectTeamMember
    {
        public int ProjectId { get; set; }
        public int MemberId { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LeftAt { get; set; }

        // Navigation properties
        public Project? Project { get; set; }
        public TeamMember? TeamMember { get; set; }
    }
} 